using Cysharp.Threading.Tasks;

namespace JFrame.Common
{
    public interface ITransitionProvider
    {
        UniTask<ITransition>  InstantiateAsync(string transitionType);
    }
}
