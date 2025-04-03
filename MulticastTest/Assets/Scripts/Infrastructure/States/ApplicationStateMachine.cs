using System.Collections.Generic;
using System.Linq;
using Infrastructure.StateMachine;
using Infrastructure.States.States;
using Zenject;

namespace Infrastructure.States
{
    public class ApplicationStateMachine : IStateSwitcher, IInitializable
    {
        protected readonly DiContainer _container;

        private List<IState> _states;
        private IState _currentState;

        public ApplicationStateMachine(DiContainer container)
        {
            _container = container;
        }

        public void SwitchState<State>() where State : IState
        {
            IState state = _states.FirstOrDefault(state => state is State);

            _currentState.Exit();
            _currentState = state;
            _currentState.Enter();
        }

        public void Initialize()
        {
            _states = new List<IState>
            {
                _container.Resolve<InitializeState>(),
                _container.Resolve<MenuState>(),
                _container.Resolve<GameState>()
            };

            _currentState = _states[0];
            _currentState.Enter();
        }
    }
}
