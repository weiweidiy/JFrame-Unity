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


        ITransition transition;


        public async void TransitionToScene(string sceneName, string transitionType)
        {
            transition = await transitionProvider.InstantiateAsync(transitionType);
            await transition.TransitionOut();

            await sceneController.Switch(sceneName);

            sceneController.onSceneViewLoadedCompleted += OnViewCompleted;

        }

        private async void OnViewCompleted()
        {
            await transition.TransitionIn();

            sceneController.onSceneViewLoadedCompleted -= OnViewCompleted;
        }


    }
}
