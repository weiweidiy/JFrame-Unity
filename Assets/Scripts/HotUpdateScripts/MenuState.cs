using Adic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace JFrame.Game.HotUpdate
{


    public class MenuState : GameBaseState
    {
        
        internal async UniTask OnEnter(bool isRestart)
        {
            Debug.Log("MenuState OnEnter " + isRestart);
            Debug.Assert(Owner != null, "owner is null");
            await UniTask.Delay(2000);
        }
    }
}

