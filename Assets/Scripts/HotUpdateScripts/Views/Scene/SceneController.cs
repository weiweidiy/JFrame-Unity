
using Adic;
using JFrame.Common;
using JFrame.Game.Models;
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
        ITransitionProvider transitionProvider;

        ITransition transition;

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
            SwitchToMain(false);
        }

        /// <summary>
        /// 开始菜单
        /// </summary>
        /// <param name="isRestart"></param>
        public async void SwitchToMain(bool isRestart , string transitionName = "")
        {
            //切换场景
            Debug.Log("ToMain");
            if(isRestart && transitionName != "")
            {
                transition = await transitionProvider.InstantiateAsync(transitionName);
                await transition.TransitionOut();
                sm.SwitchToMain(isRestart);
            }
            else
            {
                sm.SwitchToMain(isRestart);
            }         
        }

        /// <summary>
        /// 开始游戏
        /// </summary>
        public async void SwitchToBattle(PlayerAccount account, string transitionName = "")
        {
            //切换场景
            Debug.Log("ToBattle");
            if(transitionName != "")
            {
                transition = await transitionProvider.InstantiateAsync(transitionName);
                await transition.TransitionOut();
                sm.SwitchToBattle(account);
            }
            else
            {
                sm.SwitchToBattle(account);
            }
            
        }


        void OnSceneTransitioned(SceneBaseState source , SceneBaseState target)
        {
            onSceneTransition?.Invoke(source.Name, target.Name);
            Debug.Log("OnStateChanged" + target.Name);
        }

        private async void OnSceneEnter(SceneBaseState scene)
        {
            if(transition != null)
                await transition.TransitionIn();

            onSceneEnter?.Invoke(scene.Name);
        }

        private void OnSceneExit(SceneBaseState scene)
        {
            onSceneExit?.Invoke(scene.Name);
        }
    }
}

