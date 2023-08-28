
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
    public class SceneController
    {
        public event Action<string, string> onSceneTransition;
        public event Action<string> onSceneEnter;
        public event Action<string> onSceneExit;

        public event Action onSceneViewCompleted;

        [Inject]
        SceneSM sm;

        [Inject]
        IInjectionContainer container;

        [Inject]
        void Init()
        {
            Debug.Log("GameManager init");
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

            Switch(GameDefine.SCENE_MAIN);
        }

        public UniTask Switch(string sceneName)
        {
            switch(sceneName)
            {
                case GameDefine.SCENE_MAIN:
                    return sm.SwitchToMain();
                    
                case GameDefine.SCENE_BATTLE:
                    return sm.SwitchToBattle();

                default:
                    return UniTask.DelayFrame(1);
            }
        }


        void OnSceneTransitioned(SceneBaseState source, SceneBaseState target)
        {
            onSceneTransition?.Invoke(source.Name, target.Name);
            Debug.Log("OnStateChanged" + target.Name);
        }

        private async void OnSceneEnter(SceneBaseState scene)
        {
            onSceneEnter?.Invoke(scene.Name);

            var sceneView = GetSceneUIController(scene.Name);
            await sceneView.Show();
            onSceneViewCompleted?.Invoke();
        }

        private void OnSceneExit(SceneBaseState scene)
        {
            onSceneExit?.Invoke(scene.Name);
        }

        ViewController GetSceneUIController(string sceneName)
        {
            switch (sceneName)
            {
                case GameDefine.SCENE_MAIN:
                    return container.Resolve<MainSceneViewController>();
                case GameDefine.SCENE_BATTLE:
                    return container.Resolve<BattleSceneViewController>();
                default:
                    return null;
            }
        }
    }
}

