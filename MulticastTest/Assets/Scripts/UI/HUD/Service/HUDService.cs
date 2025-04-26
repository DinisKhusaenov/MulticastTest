using System.Collections.Generic;
using GameLogic.Gameplay.GameLogic;
using Gameplay.Clusters;
using Gameplay.Levels;
using Infrastructure.States;
using Infrastructure.States.States;
using UI.HUD.Windows;
using UI.HUD.Windows.Factory;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD.Service
{
    public class HUDService : IHUDService
    {
        private readonly ILevelCompletionChecker _levelCompletionChecker;
        private readonly IWindowFactory _windowFactory;
        private readonly ApplicationStateMachine _applicationStateMachine;
        private ILevelSessionService _levelSessionService;
        
        private Button _checkLevelButton;
        private Button _quitButton;
        private IReadOnlyList<IClusterContainer> _containers;
        private Level _currentLevel;
        private Canvas _canvas;
        private IGameOverView _gameOverView;

        public HUDService(
            ILevelCompletionChecker levelCompletionChecker, 
            IWindowFactory windowFactory, 
            ApplicationStateMachine applicationStateMachine)
        {
            _levelCompletionChecker = levelCompletionChecker;
            _windowFactory = windowFactory;
            _applicationStateMachine = applicationStateMachine;
        }

        public void Initialize(Button checkLevelButton, ILevelSessionService levelSessionService, Canvas canvas, Button quitButton)
        {
            _canvas = canvas;
            _checkLevelButton = checkLevelButton;
            _levelSessionService = levelSessionService;
            _quitButton = quitButton;
            
            _checkLevelButton.onClick.AddListener(CheckLevel);
            _quitButton.onClick.AddListener(LeaveToMenu);
        }

        public void InitializeByLevel(IReadOnlyList<IClusterContainer> containers, Level currentLevel)
        {
            _containers = containers;
            _currentLevel = currentLevel;
        }

        public void Dispose()
        {
            _checkLevelButton.onClick.RemoveListener(CheckLevel);
            _quitButton.onClick.RemoveListener(LeaveToMenu);
        }
        
        private async void CheckLevel()
        {
            if (_levelCompletionChecker.IsCompleted(_containers, _currentLevel))
            {
                _gameOverView ??= await _windowFactory.CreateGameOverView(_canvas);
                _gameOverView.Show(GetWordsFromContainer());
                _gameOverView.NextLevelClicked += StartNextLevel;
                _gameOverView.MenuClicked += LeaveToMenu;
            }
        }

        private void RemoveCallbacks()
        {
            if (_gameOverView == null) return;
            _gameOverView.NextLevelClicked -= StartNextLevel;
            _gameOverView.MenuClicked -= LeaveToMenu;
        }

        private List<string> GetWordsFromContainer()
        {
            List<string> words = new();

            foreach (IClusterContainer container in _containers)
            {
                words.Add(container.GetWordFromClusters());
            }

            return words;
        }

        private void StartNextLevel()
        {
            _gameOverView.Hide();
            _levelSessionService.PrepareNextLevel();
            _levelSessionService.Run();

            RemoveCallbacks();
        }

        private void LeaveToMenu()
        {
            RemoveCallbacks();
            _applicationStateMachine.SwitchState<MenuState>();
        }
        
    }
}