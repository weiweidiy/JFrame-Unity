
using Adic;
using JFrame.Game.Model;
using Stateless;
using System;
using UnityEngine;

namespace JFrame.Game.View
{
    public class SceneController
    {
        public event Action<string, string> onSceneTransition;
        public event Action<string> onSceneEnter;
        public event Action<string> onSceneExit;

        [Inject]
        SceneSM sm;

        [Inject]
        void Init()
        {
            Debug.Log("GameManager init");
            sm.Initialize(this);
            sm.onSceneTransition += OnStateChanged;
            sm.onSceneEnter += OnSceneEnter;
            sm.onSceneExit += OnSceneExit;
        }


        /// <summary>
        /// 启动游戏
        /// </summary>
        public void Run()
        {
            SwitchToMain(false);
        }

        /// <summary>
        /// 开始菜单
        /// </summary>
        /// <param name="isRestart"></param>
        public void SwitchToMain(bool isRestart)
        {
            //切换场景
            Debug.Log("ToMain");
            sm.SwitchToMain(isRestart);
        }

        /// <summary>
        /// 开始游戏
        /// </summary>
        public void SwitchToBattle(PlayerAccount account)
        {
            //切换场景
            Debug.Log("ToBattle");
            sm.SwitchToBattle(account);
        }


        void OnStateChanged(SceneBaseState source , SceneBaseState target)
        {
            onSceneTransition?.Invoke(source.Name, target.Name);
            Debug.Log("OnStateChanged" + target.Name);
        }

        private void OnSceneEnter(SceneBaseState scene)
        {
            onSceneEnter?.Invoke(scene.Name);
        }

        private void OnSceneExit(SceneBaseState scene)
        {
            onSceneExit?.Invoke(scene.Name);
        }
    }
}

