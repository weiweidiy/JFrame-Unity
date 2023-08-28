using Adic;
using Cysharp.Threading.Tasks;
using JFrame.Game.Models;
using UnityEngine;

namespace JFrame.Game.View
{
    public class BattleSceneViewController : ViewController
    {
        [Inject]
        SceneController sceneController;

        [Inject]
        PlayerAccount playerAccount;

        [Inject]
        void Init()
        {
            sceneController.onSceneEnter += SceneController_onSceneEnter;
        }

        private async void SceneController_onSceneEnter(string scene)
        {
            if (!scene.Equals("Battle"))
                return;

            await Show();
        }

        protected override UniTask DoShow()
        {
            Debug.Log("battle ui run 初始化battle ui" + playerAccount.Account);
            return UniTask.DelayFrame(300);
        }


    }
}
