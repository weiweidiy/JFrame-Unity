
using StateMachine;

namespace JFrame.Game.HotUpdate
{
    public class GameSM
    {
        enum Trigger
        {
            StartGame,
            StartMenu
        }

        enum State
        {
            Menu,
            Game,
        }

        State _state = State.Menu;

        StateMachine<State, Trigger> machine;
        StateMachine<State, Trigger>.TriggerWithParameters<bool> startMenuTrigger;
        //StateMachine<State, Trigger>.TriggerWithParameters<int> _setVolumeTrigger;
        //StateMachine<State, Trigger>.TriggerWithParameters<string> _setCalleeTrigger;

        public GameSM()
        {
            //配置游戏状态机
            machine = new StateMachine<State, Trigger>(() => _state, s => _state = s);
            startMenuTrigger = machine.SetTriggerParameters<bool>(Trigger.StartMenu);
            //_setCalleeTrigger = _machine.SetTriggerParameters<string>(Trigger.CallDialed);

            machine.Configure(State.Menu)
                .Permit(Trigger.StartGame, State.Game);

            machine.Configure(State.Game)
                .Permit(Trigger.StartMenu, State.Menu);
        }

        public void StartGame()
        {
            machine.Fire(Trigger.StartGame);
        }

        public void ReturnMenu()
        {
            machine.Fire(Trigger.StartMenu);
        }
    }
}

