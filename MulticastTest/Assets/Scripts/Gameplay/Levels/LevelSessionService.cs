using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameLogic.Gameplay.GameLogic;
using Gameplay.Clusters;
using Gameplay.Clusters.Factory;
using Gameplay.StaticData;
using Infrastructure.Loading.Level;
using UnityEngine;

namespace Gameplay.Levels
{
    public class LevelSessionService : ILevelSessionService
    {
        private readonly IWordsContainerFactory _wordsContainerFactory;
        private readonly IClusterFactory _clusterFactory;
        private readonly IStaticDataService _staticDataService;
        private readonly ILevelDataLoader _levelDataLoader;
        private readonly IClustersGenerator _clustersGenerator;
        
        private readonly List<WordsContainer> _words = new();
        private readonly List<Cluster> _clusters = new();
        
        private Transform _clustersParent;
        private Transform _wordsParent;
        private LevelsData _levelsData;
        private int _currentLevel;

        public LevelSessionService(
            IWordsContainerFactory wordsContainerFactory, 
            IClusterFactory clusterFactory, 
            IStaticDataService staticDataService,
            ILevelDataLoader levelDataLoader,
            IClustersGenerator clustersGenerator)
        {
            _wordsContainerFactory = wordsContainerFactory;
            _clusterFactory = clusterFactory;
            _staticDataService = staticDataService;
            _levelDataLoader = levelDataLoader;
            _clustersGenerator = clustersGenerator;
        }

        public void SetUp(Transform clustersParent, Transform wordsParent)
        {
            _clustersParent = clustersParent;
            _wordsParent = wordsParent;
        }

        public async UniTask Run()
        {
            await LoadLevels();
            CreateContainers().Forget();
            CreateClusters().Forget();
        }

        public void CleanUp()
        {
        }

        private async UniTask LoadLevels()
        {
            _levelsData = await _levelDataLoader.LoadDataAsync();
        }

        private async UniTask CreateContainers()
        {
            for (int i = 0; i < GetWordsCount(); i++)
            {
                var container = await _wordsContainerFactory.CreateWordsContainer(_wordsParent);
                _words.Add(container);
            }
        }

        private async UniTask CreateClusters()
        {
            for (int i = 0; i < GetWordsCount(); i++)
            {
                foreach (string clusterText in _clustersGenerator.GetClusterBy(_levelsData.Levels[_currentLevel]))
                {
                    Cluster cluster = await _clusterFactory.CreateCluster(_clustersParent, clusterText);
                    _clusters.Add(cluster);
                }
            }
        }

        private int GetWordsCount()
        {
            return Math.Min(
                _levelsData.Levels[_currentLevel].Words.Count,
                _staticDataService.LevelConfig.WordsCount);
        }
    }
}