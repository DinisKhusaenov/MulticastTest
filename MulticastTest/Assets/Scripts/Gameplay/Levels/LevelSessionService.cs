using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameLogic.Gameplay.GameLogic;
using Gameplay.Clusters;
using Gameplay.Clusters.Factory;
using Gameplay.Placer;
using Gameplay.StaticData;
using Infrastructure.Loading.Level;
using UI.HUD.Service;
using UnityEngine;

namespace Gameplay.Levels
{
    public class LevelSessionService : ILevelSessionService
    {
        private readonly IClustersContainerFactory _clustersContainerFactory;
        private readonly IClusterFactory _clusterFactory;
        private readonly IStaticDataService _staticDataService;
        private readonly ILevelDataLoader _levelDataLoader;
        private readonly ILevelCleanUpService _levelCleanUpService;
        private readonly IHUDService _hudService;

        private readonly List<IClusterContainer> _containers = new();
        private readonly List<ICluster> _clusters = new();
        
        private IClustersGenerator _clustersGenerator;
        private IClusterPlacer _clusterPlacer;
        private IClustersInitialContainer _clustersInitialContainer;
        private Transform _wordsParent;
        private Transform _moveParent;
        private LevelsData _levelsData;
        private int _currentLevel;

        public LevelSessionService(
            IClustersContainerFactory clustersContainerFactory, 
            IClusterFactory clusterFactory, 
            IStaticDataService staticDataService,
            ILevelDataLoader levelDataLoader,
            ILevelCleanUpService levelCleanUpService,
            IHUDService hudService)
        {
            _clustersContainerFactory = clustersContainerFactory;
            _clusterFactory = clusterFactory;
            _staticDataService = staticDataService;
            _levelDataLoader = levelDataLoader;
            _levelCleanUpService = levelCleanUpService;
            _hudService = hudService;
        }

        public void SetUp(IClustersInitialContainer clustersInitialContainer, Transform wordsParent, Transform moveParent)
        {
            _moveParent = moveParent;
            _clustersInitialContainer = clustersInitialContainer;
            _wordsParent = wordsParent;
            
            _clustersGenerator = new ClustersGenerator(
                _staticDataService.LevelConfig.MinClusterLength,
                _staticDataService.LevelConfig.MaxClusterLength);
        }

        public async UniTask Run()
        {
            _levelsData ??= await LoadLevels();
            await CreateContainers();
            await CreateClusters();

            _clusterPlacer = new ClusterPlacer(
                _moveParent, 
                _clustersInitialContainer, 
                _containers,
                _clusters);
            
            _hudService.InitializeByLevel(_containers, _levelsData.Levels[_currentLevel]);
            _levelCleanUpService.Initialize(_containers, _clusters);
        }

        public void CleanUp()
        {
            _levelCleanUpService.CleanUp();
            _clusterPlacer = null;
            _clusters.Clear();
            _containers.Clear();
        }

        public void PrepareNextLevel()
        {
            CleanUp();
            _currentLevel++;

            if (_currentLevel >= _levelsData.Levels.Count)
                _currentLevel = 0;
        }

        private async UniTask<LevelsData> LoadLevels()
        {
            return await _levelDataLoader.LoadDataAsync();
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
                ICluster cluster = await _clusterFactory.CreateCluster(_clustersInitialContainer.Container, clusterText);
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