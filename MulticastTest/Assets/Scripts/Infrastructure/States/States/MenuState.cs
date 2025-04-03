using Infrastructure.Loading.Scene;

namespace Infrastructure.States.States
{
    public class MenuState : IState
    {
        private readonly ApplicationStateMachine _applicationStateMachine;
        private readonly ISceneLoadService _sceneLoadService;

        public MenuState(ApplicationStateMachine applicationStateMachine, ISceneLoadService sceneLoadService)
        {
            _applicationStateMachine = applicationStateMachine;
            _sceneLoadService = sceneLoadService;
        }

        public void Enter()
        {
            _sceneLoadService.LoadScene(SceneNames.Menu, OnSceneLoaded);
        }

        public void Exit()
        {
        }

        private void OnSceneLoaded()
        {

        }
    }
}
