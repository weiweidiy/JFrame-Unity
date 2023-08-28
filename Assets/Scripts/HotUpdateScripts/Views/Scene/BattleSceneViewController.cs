using Adic;
using Cysharp.Threading.Tasks;
using JFrame.Game.Models;
using UnityEngine;

namespace JFrame.Game.View
{
    public class BattleSceneViewController : ViewController
    {
        [Inject]
        PlayerAccount playerAccount;

        [Inject]
        void Init()
        {
        }


        protected override UniTask DoShow()
        {
            Debug.Log("battle ui run 初始化battle ui" + playerAccount.Account);
            return UniTask.DelayFrame(300);
        }


    }
}
