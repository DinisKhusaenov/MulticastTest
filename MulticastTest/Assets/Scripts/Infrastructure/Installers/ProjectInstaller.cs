using Gameplay.Cameras;
using Gameplay.Input;
using Gameplay.Sound;
using Gameplay.StaticData;
using Infrastructure.AssetManagement;
using Infrastructure.Loading.Scene;
using Infrastructure.PoolService.Factory;
using Infrastructure.RemoteConfig;
using Infrastructure.Services.LogService;
using Infrastructure.States;
using Infrastructure.States.States;
using UI.HUD.Windows;
using UI.HUD.Windows.Factory;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private LoadingCurtain _loadingCurtain;
        [SerializeField] private SoundService _soundService;
        
        public override void InstallBindings()
        {
            BindSceneLoader();
            BindApplicationStateMachine();
            BindServices();
            BindCameraProvider();
            BindInputService();
            BindGameplayServices();
            BindUI();
            BindLoadingCurtain();
            BindFactory();
        }

        private void BindFactory()
        {
            Container.Bind<IPoolFactory>().To<PoolFactory>().AsSingle();
        }

        private void BindLoadingCurtain()
        {
            Container.BindInterfacesAndSelfTo<LoadingCurtain>().FromComponentInNewPrefab(_loadingCurtain).AsSingle();
        }

        private void BindUI()
        {
            Container.Bind<IWindowFactory>().To<WindowFactory>().AsSingle();
        }

        private void BindServices()
        {
            Container.Bind<ILogService>().To<LogService>().AsSingle();
            Container.BindInterfacesAndSelfTo<SoundService>().FromComponentInNewPrefab(_soundService).AsSingle();
        }

        private void BindSceneLoader()
        {
            Container.BindInterfacesTo<SceneLoader>().AsSingle();
        }

        private void BindApplicationStateMachine()
        {
            var stateMachine = new ApplicationStateMachine(Container);
            Container.BindInterfacesAndSelfTo<ApplicationStateMachine>().FromInstance(stateMachine).AsSingle();

            Container.BindInterfacesAndSelfTo<InitializeState>().AsSingle().WithArguments(stateMachine);
            Container.BindInterfacesAndSelfTo<MenuState>().AsSingle().WithArguments(stateMachine);
            Container.BindInterfacesAndSelfTo<GameState>().AsSingle().WithArguments(stateMachine);
        }
        
        private void BindCameraProvider()
        {
            Container.Bind<ICameraProvider>().To<CameraProvider>().AsSingle();
        }
        
        private void BindInputService()
        {
            Container.Bind<IInputService>().To<StandaloneInputService>().AsSingle();
            Container.Bind<IRemoteConfigService>().To<RemoteConfigService>().AsSingle();
        }
        
        private void BindGameplayServices()
        {
            Container.Bind<IStaticDataService>().To<StaticDataService>().AsSingle();
            Container.Bind<IAssetProvider>().To<AssetProvider>().AsSingle();
        }
    }
}