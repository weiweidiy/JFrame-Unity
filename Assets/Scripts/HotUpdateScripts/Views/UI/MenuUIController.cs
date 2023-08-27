using Adic;
using Cysharp.Threading.Tasks;
using JFrame.Common;
using JFrame.Game.Model;
using UnityEngine;

namespace JFrame.Game.View
{
    public class MenuUIController : UIController
    {
        [Inject]
        SceneController sceneController;

        [Inject]
        PlayerAccount playerAccount;

        [Inject]
        IAssetsLoader assetLoader;

        [Inject]
        void Init()
        {
            sceneController.onSceneEnter += SceneController_onSceneEnter;
        }

        public override async UniTask Run()
        {
            Debug.Log("Main ui run 初始化main ui");
            //创建panel等ui
            var menu = assetLoader.Instantiate("Menu");
            playerAccount.account = "Jichunwei";
            await UniTask.DelayFrame(1);
        }

        private async void SceneController_onSceneEnter(string scene)
        {
            if (!scene.Equals("Main"))
                return;

            await Run();
        }
    }
}
