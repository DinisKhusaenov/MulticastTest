using Infrastructure.EntryPoints;
using Infrastructure.Loading.Scene;
using UnityEngine;

namespace Infrastructure.States.States
{
    public class GameState : IState
    {
        private readonly ApplicationStateMachine _applicationStateMachine;
        private readonly ISceneLoadService _sceneLoadService;

        public GameState(ApplicationStateMachine applicationStateMachine, ISceneLoadService sceneLoadService)
        {
            _applicationStateMachine = applicationStateMachine;
            _sceneLoadService = sceneLoadService;
        }

        public void Enter()
        {
            _sceneLoadService.LoadScene(SceneNames.Game, OnLoaded);
        }

        private void OnLoaded()
        {
        }

        public void Exit()
        {
        }

    }
}
