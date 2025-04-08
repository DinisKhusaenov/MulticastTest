using Gameplay.Clusters;
using Gameplay.Levels;
using UI.HUD.Service;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Infrastructure.EntryPoints
{
    public class GameEntryPoint : MonoBehaviour
    {
        [SerializeField] private ClustersInitialContainer _clusterParent;
        [SerializeField] private Transform _wordsParent;
        [SerializeField] private Transform _moveClusterParent;
        [SerializeField] private Button _checkLevelButton;
        [SerializeField] private Button _quitButton;
        [SerializeField] private Canvas _hudCanvas;
        
        private ILevelSessionService _levelSessionService;
        private IHUDService _hudService;

        [Inject]
        private void Construct(ILevelSessionService levelSessionService, IHUDService hudService)
        {
            _hudService = hudService;
            _levelSessionService = levelSessionService;
        }

        private void Start()
        {
            _levelSessionService.SetUp(_clusterParent, _wordsParent, _moveClusterParent);
            _levelSessionService.Run();
            _hudService.Initialize(_checkLevelButton, _levelSessionService, _hudCanvas, _quitButton);
        }
    }
}