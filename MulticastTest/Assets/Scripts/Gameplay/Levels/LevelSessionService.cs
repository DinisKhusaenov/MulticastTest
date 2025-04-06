using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameLogic.Gameplay.GameLogic;
using Gameplay.Clusters;
using Gameplay.Clusters.Factory;
using Gameplay.Placer;
using Gameplay.StaticData;
using Infrastructure.Loading.Level;
using UnityEngine;

namespace Gameplay.Levels
{
    public class LevelSessionService : ILevelSessionService
    {
        private readonly IClustersContainerFactory _clustersContainerFactory;
        private readonly IClusterFactory _clusterFactory;
        private readonly IStaticDataService _staticDataService;
        private readonly ILevelDataLoader _levelDataLoader;
        
        private readonly List<ClustersClusterContainer> _containers = new();
        private readonly List<Cluster> _clusters = new();
        
        private IClustersGenerator _clustersGenerator;
        private IClusterPlacer _clusterPlacer;
        private Transform _clustersParent;
        private Transform _wordsParent;
        private Transform _moveParent;
        private LevelsData _levelsData;
        private int _currentLevel;

        public LevelSessionService(
            IClustersContainerFactory clustersContainerFactory, 
            IClusterFactory clusterFactory, 
            IStaticDataService staticDataService,
            ILevelDataLoader levelDataLoader)
        {
            _clustersContainerFactory = clustersContainerFactory;
            _clusterFactory = clusterFactory;
            _staticDataService = staticDataService;
            _levelDataLoader = levelDataLoader;
        }

        public void SetUp(Transform clustersParent, Transform wordsParent, Transform moveParent)
        {
            _moveParent = moveParent;
            _clustersParent = clustersParent;
            _wordsParent = wordsParent;
            
            _clustersGenerator = new ClustersGenerator(
                _staticDataService.LevelConfig.MinClusterLength,
                _staticDataService.LevelConfig.MaxClusterLength);
        }

        public async UniTask Run()
        {
            await LoadLevels();
            await CreateContainers();
            await CreateClusters();

            _clusterPlacer = new ClusterPlacer(
                _moveParent, 
                _clustersParent, 
                _containers,
                _clusters);
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
                var container = await _clustersContainerFactory.CreateClustersContainer(_wordsParent);
                _containers.Add(container);
            }
        }

        private async UniTask CreateClusters()
        {
            List<string> clusterTexts = _clustersGenerator.GetClusterBy(_levelsData.Levels[_currentLevel]);
            foreach (string clusterText in clusterTexts)
            {
                Cluster cluster = await _clusterFactory.CreateCluster(_clustersParent, clusterText);
                _clusters.Add(cluster);
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