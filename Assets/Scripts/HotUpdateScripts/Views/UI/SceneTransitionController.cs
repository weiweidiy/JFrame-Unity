using Adic;
using Adic.Container;
using JFrame.Common;

namespace JFrame.Game.View
{
    public class SceneTransitionController
    {
        [Inject]
        ITransitionProvider transitionProvider;

        [Inject]
        SceneController sceneController;

        [Inject]
        IInjectionContainer container;

        ITransition transition;

        ViewController ui;

        public async void TransitionToScene(string sceneName)
        {
            transition = await transitionProvider.InstantiateAsync("SMFadeTransition");
            await transition.TransitionOut();

            await sceneController.Switch(sceneName);

            ui = GetSceneUIController(sceneName);
            ui.onEnterCompleted += SceneUI_onEnterCompleted;
        }

        private async void SceneUI_onEnterCompleted(ViewController obj)
        {
            await transition.TransitionIn();

            ui.onEnterCompleted -= SceneUI_onEnterCompleted;
        }

        ViewController GetSceneUIController(string sceneName)
        {
            switch(sceneName)
            {
                case "Main":
                    return container.Resolve<MainSceneViewController>();
                case "Battle":
                    return container.Resolve<BattleSceneViewController>();
                default:
                    return null;
            }
        }
    }
}
