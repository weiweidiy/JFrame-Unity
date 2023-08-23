using Cysharp.Threading.Tasks;
using UnityEngine;

namespace JFrame.Game.HotUpdate
{


    public class MenuState : State
    {
        public MenuState(GameManager owner)
        {

        }

        internal async UniTask OnEnter(bool isRestart)
        {
            Debug.Log("MenuState OnEnter " + isRestart);
            await UniTask.Delay(2000);
        }
    }
}

