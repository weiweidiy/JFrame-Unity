using Adic;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace JFrame.Game.View
{
    public abstract class ViewController
    {
        /// <summary>
        /// ������ͼ������������
        /// </summary>
        public event Action<ViewController> onLoadedCompleted;

        [Inject]
        protected virtual void Init()
        {
        }

        /// <summary>
        /// ��ʾ
        /// </summary>
        /// <returns></returns>
        public async UniTask Show()
        {
            await DoShow();

            onLoadedCompleted?.Invoke(this);
        }

        /// <summary>
        /// ʵ����ʾ�Ĵ��룬����ʵ��
        /// </summary>
        /// <returns></returns>
        protected abstract UniTask DoShow();

    }
}
