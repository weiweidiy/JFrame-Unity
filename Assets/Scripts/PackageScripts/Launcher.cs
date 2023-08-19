using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using YooAsset;

namespace JFrame.Game
{
    public class Launcher : MonoBehaviour
    {
        // Start is called before the first frame update
        async void Start()
        {
            //播放splash
            await PlaySplash();

            SceneManager.LoadScene("Main");
        }

        /// <summary>
        /// 播放splash动画
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private async UniTask<bool> PlaySplash()
        {
            await UniTask.Delay(3000);
            return true;
        }

    }
}
