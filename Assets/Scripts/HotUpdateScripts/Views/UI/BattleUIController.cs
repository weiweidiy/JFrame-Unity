using Adic;
using Cysharp.Threading.Tasks;
using JFrame.Game.Models;
using UnityEngine;

namespace JFrame.Game.View
{
    public class BattleUIController : UIController
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

        public override UniTask Open()
        {
            Debug.Log("battle ui run 初始化battle ui" + playerAccount.Account);
            return UniTask.DelayFrame(1);
        }

        private void SceneController_onSceneEnter(string scene)
        {
            if (!scene.Equals("Battle"))
                return;

            Open();
        }
    }
}
