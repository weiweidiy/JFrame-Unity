using JFrame.Game.Core;

namespace JFrame.Game.Models
{
    public abstract class Model : IModel, INotifier
    {
        public void SendEvent<TArg>(string name, TArg arg)
        {
            throw new System.NotImplementedException();
        }
    }
}

