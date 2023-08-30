using Adic;
using JFrame.Game.View;

namespace JFrame.Game.Commands
{
    public class StartBattle : Command
    {
        [Inject]
        SceneTransitionController sceneController;

        public override void Execute(params object[] parameters)
        {
            sceneController.TransitionToScene(Scene.BATTLE, SceneTransition.FADE);
        }
    }
}


