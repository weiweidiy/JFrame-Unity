using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Adic;
using JFrame.Game.View;

namespace JFrame.Game.Commands
{
    public class RunGame : Command
    {
        [Inject]
        SceneController sceneController;

        public override void Execute(params object[] parameters)
        {
            sceneController.Run();
        }
    }


    public class StartBattle : Command
    {
        [Inject]
        SceneController sceneController;

        public override void Execute(params object[] parameters)
        {
            
        }
    }
}


