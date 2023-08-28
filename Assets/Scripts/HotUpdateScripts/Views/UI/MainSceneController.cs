using Adic;
using Cysharp.Threading.Tasks;
using JFrame.Common;
using JFrame.Game.Models;
using UnityEngine;

namespace JFrame.Game.View
{
    //public class MainSceneControllers : UIController
    //{
    //    public override UniTask Open()
    //    {
    //        throw new System.NotImplementedException();
    //    }
    //}


    public class MainSceneController : UIController
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

        public override async UniTask Open()
        {
            Debug.Log("Main ui run 初始化main ui");
            //创建panel等ui
            var menu = assetLoader.InstantiateAsync("Menu");
            playerAccount.Account = "Jichunwei";
            await UniTask.DelayFrame(1);
        }

        private async void SceneController_onSceneEnter(string scene)
        {
            if (!scene.Equals("Main"))
                return;

            await Open();
        }
    }
}
