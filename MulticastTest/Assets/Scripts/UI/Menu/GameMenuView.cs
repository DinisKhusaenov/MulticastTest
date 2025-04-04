using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Menu
{
    public class GameMenuView : MonoBehaviour
    {
        [SerializeField] private Button _startButton;

        private IGameMenuPresenter _gameMenuPresenter;

        [Inject]
        private void Construct(IGameMenuPresenter gameMenuPresenter)
        {
            _gameMenuPresenter = gameMenuPresenter;
            
            _startButton.onClick.AddListener(OnStartClicked);
        }

        private void OnDestroy()
        {
            _startButton.onClick.RemoveListener(OnStartClicked);
        }

        private void OnStartClicked()
        {
            _gameMenuPresenter.StartGame();
        }
    }
}