using Gameplay.Cameras;
using Gameplay.Input;
using Infrastructure.Loading.Scene;
using Infrastructure.Services.LogService;
using Infrastructure.States;
using Infrastructure.States.States;
using Zenject;

namespace Infrastructure.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindSceneLoader();
            BindApplicationStateMachine();
            BindServices();
            BindCameraProvider();
            BindInputService();
        }

        private void BindServices()
        {
            Container.Bind<ILogService>().To<LogService>().AsSingle();
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
        }
    }
}