
using Adic;
using StateMachine;
using System;
using UnityEngine;

namespace JFrame.Game.HotUpdate
{
    public class GameManager
    {
        [Inject]
        GameSM sm;

        [Inject]
        void Initialize()
        {
            sm.Initialize(this);     
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
    }
}

