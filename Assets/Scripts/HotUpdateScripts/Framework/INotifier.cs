namespace JFrame.Game.Core
{
    public interface INotifier
    {
        void SendEvent<TArg>(string name, TArg arg);
    }
}

