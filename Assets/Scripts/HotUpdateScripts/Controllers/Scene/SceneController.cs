
using Adic;
using Stateless;
using System;
using UnityEngine;

namespace JFrame.Game.HotUpdate
{
    public class SceneController
    {
        public event Action<string, string> onStateChanged;

        [Inject]
        SceneSM sm;

        [Inject]
        void Initialize()
        {
            Debug.Log("GameManager init");
            sm.Initialize(this);
            sm.onStateChanged += OnStateChanged;
        }

        /// <summary>
        /// 启动游戏
        /// </summary>
        public void Run()
        {
            ToMenu(false);
        }

        /// <summary>
        /// 开始菜单
        /// </summary>
        /// <param name="isRestart"></param>
        public void ToMenu(bool isRestart)
        {
            //切换场景
            Debug.Log("ToMenu");
            sm.SwitchToMenu(isRestart);
        }

        /// <summary>
        /// 开始游戏
        /// </summary>
        public void ToGame(PlayerAccount account)
        {
            //切换场景
            Debug.Log("ToGame");
            sm.SwitchToGame(account);
        }


        void OnStateChanged(SceneBaseState source , SceneBaseState target)
        {
            onStateChanged?.Invoke(source.Name, target.Name);
            Debug.Log("OnStateChanged" + target.Name);
        }
    }
}

