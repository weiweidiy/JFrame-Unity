using UnityEngine;

namespace JFrame.Game.HotUpdate
{
    public class GameState : State
    {
        public GameState(GameManager owner)
        {

        }

        internal void OnEnter(PlayerAccount playerAccount)
        {
            Debug.Log("GameState OnEnter " + playerAccount.account);
        }
    }
}

