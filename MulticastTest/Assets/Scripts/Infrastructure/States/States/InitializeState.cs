using Infrastructure.Loading.Scene;

namespace Infrastructure.States.States
{
    public class InitializeState : IState
    {
        private readonly ApplicationStateMachine _applicationStateMachine;
        private readonly ISceneLoadService _sceneLoadService;

        public InitializeState(ApplicationStateMachine applicationStateMachine, ISceneLoadService sceneLoadService)
        {
            _applicationStateMachine = applicationStateMachine;
            _sceneLoadService = sceneLoadService;
        }

        public void Enter()
        {
            _sceneLoadService.LoadScene(SceneNames.Initialize, OnSceneLoaded);
        }

        public void Exit()
        {
        }

        private void OnSceneLoaded()
        {
            _applicationStateMachine.SwitchState<MenuState>();
        }
    }
}
