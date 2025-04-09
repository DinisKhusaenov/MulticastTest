using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Menu
{
    public class GameMenuView : MonoBehaviour
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _settings;

        private IGameMenuPresenter _gameMenuPresenter;

        [Inject]
        private void Construct(IGameMenuPresenter gameMenuPresenter)
        {
            _gameMenuPresenter = gameMenuPresenter;
            
            _startButton.onClick.AddListener(OnStartClicked);
            _settings.onClick.AddListener(OnSettingsClicked);
        }

        private void OnDestroy()
        {
            _startButton.onClick.RemoveListener(OnStartClicked);
            _settings.onClick.RemoveListener(OnSettingsClicked);
        }

        private void OnStartClicked()
        {
            _gameMenuPresenter.StartGame();
        }

        private void OnSettingsClicked()
        {
            _gameMenuPresenter.ShowSettings(transform).Forget();
        }
    }
}