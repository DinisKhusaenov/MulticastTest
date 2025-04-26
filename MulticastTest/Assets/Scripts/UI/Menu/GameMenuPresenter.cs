using System;
using Cysharp.Threading.Tasks;
using Gameplay.Sound;
using Gameplay.Sound.Config;
using Gameplay.StaticData;
using Infrastructure.StateMachine;
using Infrastructure.States.States;
using UI.HUD.Windows.Factory;
using UI.Menu.Settings;
using UnityEngine;
using Zenject;

namespace UI.Menu
{
    public class GameMenuPresenter : IGameMenuPresenter, IInitializable, IDisposable
    {
        private readonly IGameMenuView _gameMenuView;
        private readonly IStateSwitcher _stateSwitcher;
        private readonly ISoundService _soundService;
        private readonly IWindowFactory _windowFactory;

        private ISettingsView _settingsView;

        public GameMenuPresenter(
            IGameMenuView gameMenuView,
            IStateSwitcher stateSwitcher, 
            ISoundService soundService, 
            IWindowFactory windowFactory)
        {
            _gameMenuView = gameMenuView;
            _stateSwitcher = stateSwitcher;
            _soundService = soundService;
            _windowFactory = windowFactory;
        }
        
        public void Initialize()
        {
            _gameMenuView.StartClicked += StartGame;
            _gameMenuView.SettingsClicked += () => ShowSettings(_gameMenuView.transform).Forget();
            
            InitSound();
        }
        
        public void Dispose()
        {
            _gameMenuView.StartClicked -= StartGame;
            _gameMenuView.SettingsClicked -= () => ShowSettings(_gameMenuView.transform).Forget();
        }
        
        public void StartGame()
        {
            _stateSwitcher.SwitchState<GameState>();
        }

        public async UniTask ShowSettings(Transform parent)
        {
            _settingsView ??= await _windowFactory.CreateSettingsView(parent);
            
            _settingsView.Show();
        }

        private void InitSound()
        {
            _soundService.Play(SoundType.BackgroundMusic);
            _soundService.Mute(PlayerPrefs.GetInt(DataBaseConstants.Volume) == 0);
        }
    }
}