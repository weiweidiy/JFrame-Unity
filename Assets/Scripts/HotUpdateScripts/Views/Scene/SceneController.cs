
using Adic;
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

        [Inject]
        SceneSM sm;


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
            Switch("Main");
        }

        public UniTask Switch(string sceneName)
        {
            switch(sceneName)
            {
                case "Main":
                    return sm.SwitchToMain();
                    
                case "Battle":
                    return sm.SwitchToBattle();

                default:
                    return UniTask.DelayFrame(1);
            }
        }

        ///// <summary>
        ///// 开始菜单
        ///// </summary>
        ///// <param name="isRestart"></param>
        //public void SwitchToMain(bool isRestart, string transitionName = "")
        //{
        //    //切换场景
        //    Debug.Log("ToMain");
        //    sm.SwitchToMain(isRestart);
        //}

        ///// <summary>
        ///// 开始游戏
        ///// </summary>
        //public void SwitchToBattle(PlayerAccount account)
        //{
        //    //切换场景
        //    Debug.Log("ToBattle");
        //    sm.SwitchToBattle(account);
        //}


        void OnSceneTransitioned(SceneBaseState source, SceneBaseState target)
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

