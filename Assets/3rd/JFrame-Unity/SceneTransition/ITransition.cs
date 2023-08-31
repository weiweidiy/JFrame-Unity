using Cysharp.Threading.Tasks;

namespace JFrame.Common
{
    public interface ITransition
    {
        UniTask<SMTransitionState> TransitionOut();
        UniTask<SMTransitionState> TransitionIn();
    }
}
