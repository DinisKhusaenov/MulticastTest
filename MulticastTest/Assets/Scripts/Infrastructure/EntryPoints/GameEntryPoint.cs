using Gameplay.Levels;
using UnityEngine;
using Zenject;

namespace Infrastructure.EntryPoints
{
    public class GameEntryPoint : MonoBehaviour
    {
        [SerializeField] private Transform _clusterParent;
        [SerializeField] private Transform _wordsParent;
        [SerializeField] private Transform _moveClusterParent;
        
        private ILevelSessionService _levelSessionService;

        [Inject]
        private void Construct(ILevelSessionService levelSessionService)
        {
            _levelSessionService = levelSessionService;
        }

        private void Start()
        {
            _levelSessionService.SetUp(_clusterParent, _wordsParent, _moveClusterParent);
            _levelSessionService.Run();
        }
    }
}