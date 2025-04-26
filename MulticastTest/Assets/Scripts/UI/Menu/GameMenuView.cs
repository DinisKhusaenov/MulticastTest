using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu
{
    public class GameMenuView : MonoBehaviour, IGameMenuView
    {
        public event Action StartClicked;
        public event Action SettingsClicked;
        
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _settings;

        private void OnEnable()
        {
            _startButton.onClick.AddListener(OnStartClicked);
            _settings.onClick.AddListener(OnSettingsClicked);
        }

        private void OnDisable()
        {
            _startButton.onClick.RemoveListener(OnStartClicked);
            _settings.onClick.RemoveListener(OnSettingsClicked);
        }

        private void OnStartClicked()
        {
            StartClicked?.Invoke();
        }

        private void OnSettingsClicked()
        {
            SettingsClicked?.Invoke();
        }
    }
}