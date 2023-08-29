using Adic;
using Cysharp.Threading.Tasks;
using System;

namespace JFrame.Game.View
{
    public abstract class ViewController
    {
        /// <summary>
        /// 所有视图对象加载完成了
        /// </summary>
        public event Action<ViewController> onLoadedCompleted;

        [Inject]
        protected virtual void Init()
        {

        }

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns></returns>
        public async UniTask Show()
        {
            await DoShow();

            onLoadedCompleted?.Invoke(this);
        }

        /// <summary>
        /// 实际显示的代码，子类实现
        /// </summary>
        /// <returns></returns>
        protected abstract UniTask DoShow();

    }
}
