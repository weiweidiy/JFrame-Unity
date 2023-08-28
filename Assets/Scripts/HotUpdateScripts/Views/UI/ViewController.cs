using Cysharp.Threading.Tasks;
using System;

namespace JFrame.Game.View
{
    public abstract class ViewController
    {
        public event Action<ViewController> onEnterCompleted;

        public async UniTask Show()
        {
            await DoShow();

            onEnterCompleted?.Invoke(this);
        }

        protected abstract UniTask DoShow();

    }
}
