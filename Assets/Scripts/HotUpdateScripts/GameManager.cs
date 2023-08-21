
using UnityEngine;

namespace JFrame.Game.HotUpdate
{
    public class GameManager
    {

        

        public GameManager()
        {

        }

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

        /// <summary>
        /// 开始游戏
        /// </summary>
        public void ToGame()
        {
            //切换场景
            Debug.Log("ToGame");
        }

        public void QuitGame()
        {

        }
    }
}

