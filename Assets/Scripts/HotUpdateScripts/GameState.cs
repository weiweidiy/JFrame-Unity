using Cysharp.Threading.Tasks;
using UnityEngine;

namespace JFrame.Game.HotUpdate
{
    public class GameState : State
    {
        public GameState(GameManager owner)
        {

        }

        internal async UniTask OnEnter(PlayerAccount playerAccount)
        {
            Debug.Log("GameState OnEnter " + playerAccount.account);
            await UniTask.Delay(1000);
        }
    }
}

