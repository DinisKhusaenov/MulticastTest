using System.Collections.Generic;
using GameLogic.Gameplay.GameLogic;
using Gameplay.Clusters;
using Gameplay.Levels;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD.Service
{
    public class HUDService : IHUDService
    {
        private readonly ILevelCompletionChecker _levelCompletionChecker;
        private ILevelSessionService _levelSessionService;
        
        private Button _checkLevelButton;
        private IReadOnlyList<IClusterContainer> _containers;
        private Level _currentLevel;

        public HUDService(ILevelCompletionChecker levelCompletionChecker)
        {
            _levelCompletionChecker = levelCompletionChecker;
        }

        public void Initialize(Button checkLevelButton, ILevelSessionService levelSessionService)
        {
            _checkLevelButton = checkLevelButton;
            _levelSessionService = levelSessionService;
            
            _checkLevelButton.onClick.AddListener(CheckLevel);
        }

        public void InitializeByLevel(IReadOnlyList<IClusterContainer> containers, Level currentLevel)
        {
            _containers = containers;
            _currentLevel = currentLevel;
        }

        public void Dispose()
        {
            _checkLevelButton.onClick.RemoveListener(CheckLevel);
        }
        
        private void CheckLevel()
        {
            if (_levelCompletionChecker.IsCompleted(_containers, _currentLevel))
            {
                _levelSessionService.PrepareNextLevel();
                _levelSessionService.Run();
            }
        }
    }
}