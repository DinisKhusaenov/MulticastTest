using Infrastructure.StateMachine;
using Infrastructure.States.States;

namespace UI.Menu
{
    public class GameMenuPresenter : IGameMenuPresenter
    {
        private readonly IStateSwitcher _stateSwitcher;

        public GameMenuPresenter(IStateSwitcher stateSwitcher)
        {
            _stateSwitcher = stateSwitcher;
        }
        
        public void StartGame()
        {
            _stateSwitcher.SwitchState<GameState>();
        }
    }
}