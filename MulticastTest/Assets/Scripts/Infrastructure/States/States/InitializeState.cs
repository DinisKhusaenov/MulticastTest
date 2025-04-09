using Cysharp.Threading.Tasks;
using Infrastructure.Loading.Scene;
using Infrastructure.RemoteConfig;
using UI.HUD.Windows;

namespace Infrastructure.States.States
{
    public class InitializeState : IState
    {
        private readonly ApplicationStateMachine _applicationStateMachine;
        private readonly ISceneLoadService _sceneLoadService;
        private readonly ILoadingCurtain _loadingCurtain;
        private readonly IRemoteConfigService _remoteConfigService;

        public InitializeState(
            ApplicationStateMachine applicationStateMachine, 
            ISceneLoadService sceneLoadService, 
            ILoadingCurtain loadingCurtain,
            IRemoteConfigService remoteConfigService)
        {
            _applicationStateMachine = applicationStateMachine;
            _sceneLoadService = sceneLoadService;
            _loadingCurtain = loadingCurtain;
            _remoteConfigService = remoteConfigService;
        }

        public void Enter()
        {
            _sceneLoadService.LoadScene(SceneNames.Initialize, OnSceneLoaded);
        }

        public void Exit()
        {
        }

        private async void OnSceneLoaded()
        {
            await InitializeData();
            
            _applicationStateMachine.SwitchState<MenuState>();
        }

        private async UniTask InitializeData()
        {
            _loadingCurtain.Show();

            await _remoteConfigService.Initialize();
        }
    }
}
