using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
//using DG.Tween;

public class Launcher : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
        //播放splash
        var splashTask = PlaySplash();

        //初始化YooAssets
        var initTask= InitYooAssets();

        //等待2个任务完成
        await UniTask.WhenAll(splashTask, initTask);

        //切换场景
        SceneManager.LoadScene("Main");
    }

    /// <summary>
    /// 播放splash动画
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private UniTask PlaySplash()
    {
        return UniTask.Delay(5000);
    }

    /// <summary>
    /// 初始化YooAssets
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private UniTask InitYooAssets()
    {
        return UniTask.Delay(1000);
    }
}
