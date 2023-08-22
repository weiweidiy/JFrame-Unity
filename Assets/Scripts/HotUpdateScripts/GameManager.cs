
using StateMachine;
using UnityEngine;

namespace JFrame.Game.HotUpdate
{
    public class GameManager
    {
        /// <summary>
        /// 启动游戏
        /// </summary>
        public void Run()
        {
            

        }

        /// <summary>
        /// 开始菜单
        /// </summary>
        public void OnEnterMenu(bool isReset = false)
        {
            //切换到菜单状态
            Debug.Log("OnEnterMenu");
        }

        public void ToMenu()
        {
            //切换场景
            Debug.Log("ToMenu");
        }

        /// <summary>
        /// 开始游戏
        /// </summary>
        public void ToGame()
        {
            //切换场景
            Debug.Log("ToGame");
        }
    }
}

