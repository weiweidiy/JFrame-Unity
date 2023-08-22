
using StateMachine;

namespace JFrame.Game.HotUpdate
{
    public class GameSM
    {
        enum Trigger
        {
            //CallDialed,
            //CallConnected,
            //LeftMessage,
            //PlacedOnHold,
            //TakenOffHold,
            //PhoneHurledAgainstWall,
            //MuteMicrophone,
            //UnmuteMicrophone,
            //SetVolume

            StartGame,
        }

        enum State
        {
            //OffHook,
            //Ringing,
            //Connected,
            //OnHold,
            //PhoneDestroyed

            Menu,
            Requesting,
            Connected,
            Game,
        }

        State _state = State.Menu;

        StateMachine<State, Trigger> _machine;
        //StateMachine<State, Trigger>.TriggerWithParameters<int> _setVolumeTrigger;
        //StateMachine<State, Trigger>.TriggerWithParameters<string> _setCalleeTrigger;

        public GameSM()
        {
            //配置游戏状态机
            _machine = new StateMachine<State, Trigger>(() => _state, s => _state = s);
            //_setVolumeTrigger = _machine.SetTriggerParameters<int>(Trigger.SetVolume);
            //_setCalleeTrigger = _machine.SetTriggerParameters<string>(Trigger.CallDialed);

            _machine.Configure(State.Menu)
                .Permit(Trigger.StartGame, State.Game);
        }
    }
}

