
using Adic;
using Adic.Container;
using Cysharp.Threading.Tasks;
using JFrame.Common;
using JFrame.Game.Models;
using Stateless;
using System;
using System.Security.Principal;
using UnityEngine;

namespace JFrame.Game.View
{
    /// <summary>
    /// 场景控制器，负责切换unity场景，并且初始化场景视图
    /// </summary>
    public class SceneController
    {
        /// <summary>
        /// 场景发生切换过度了
        /// </summary>
        public event Action<string, string> onSceneTransition;

        /// <summary>
        /// 场景进入了
        /// </summary>
        public event Action<string> onSceneLoaded;

        /// <summary>
        /// 场景状态退出了
        /// </summary>
        public event Action<string> onSceneExited;


        /// <summary>
        /// 场景视图显示完成
        /// </summary>
        public event Action onSceneViewLoadedCompleted;

        /// <summary>
        /// 场景状态机
        /// </summary>
        [Inject]
        SceneSM sm;

        /// <summary>
        /// 容器
        /// </summary>
        [Inject]
        IInjectionContainer container;

        /// <summary>
        /// 初始化方法
        /// </summary>
        [Inject]
        void Init()
        {
            sm.Initialize(this);
            sm.onSceneTransition += OnSceneTransitioned;
            sm.onSceneEnter += OnSceneEnter;
            sm.onSceneExit += OnSceneExit;
        }

        /// <summary>
        /// 启动游戏
        /// </summary>
        public void Run()
        {
            container.Bind<MainSceneViewController>().ToSingleton();
            container.Bind<BattleSceneViewController>().ToSingleton();

            Switch(Scene.MAIN);
        }

        /// <summary>
        /// 切换场景
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        public UniTask Switch(string sceneName)
        {
            switch(sceneName)
            {
                case Scene.MAIN:
                    return sm.SwitchToMain();
                    
                case Scene.BATTLE:
                    return sm.SwitchToBattle();

                default:
                    return UniTask.DelayFrame(1);
            }
        }

        /// <summary>
        /// 状态发生转换时
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        void OnSceneTransitioned(SceneBaseState source, SceneBaseState target)
        {
            onSceneTransition?.Invoke(source.Name, target.Name);
            Debug.Log("OnStateChanged" + target.Name);
        }

        /// <summary>
        /// 场景进入了（unity场景加载好了)
        /// </summary>
        /// <param name="scene"></param>
        private async void OnSceneEnter(SceneBaseState scene)
        {
            onSceneLoaded?.Invoke(scene.Name);

            //获取场景view
            var sceneView = GetSceneUIController(scene.Name);
            await sceneView.Show();
            //通知view加载完成了
            onSceneViewLoadedCompleted?.Invoke();
        }

        /// <summary>
        /// 场景状态退出了
        /// </summary>
        /// <param name="scene"></param>
        private void OnSceneExit(SceneBaseState scene)
        {
            onSceneExited?.Invoke(scene.Name);
        }

        /// <summary>
        /// 获取场景视图控制器
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        ViewController GetSceneUIController(string sceneName)
        {
            switch (sceneName)
            {
                case Scene.MAIN:
                    return container.Resolve<MainSceneViewController>();
                case Scene.BATTLE:
                    return container.Resolve<BattleSceneViewController>();
                default:
                    return null;
            }
        }
    }
}

