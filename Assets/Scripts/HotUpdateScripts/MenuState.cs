using UnityEngine;

namespace JFrame.Game.HotUpdate
{


    public class MenuState : State
    {
        public MenuState(GameManager owner)
        {

        }

        internal void OnEnter(bool isRestart)
        {
            Debug.Log("MenuState OnEnter " + isRestart);
        }
    }
}

