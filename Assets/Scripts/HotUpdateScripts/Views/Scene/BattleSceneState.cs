using Adic;
using Cysharp.Threading.Tasks;
using JFrame.Common;
using JFrame.Game.Models;
using UnityEngine;

namespace JFrame.Game.View
{
    public class BattleSceneState : SceneBaseState
    {

        [Inject]
        IAssetsLoader assetLoader;


        public override string Name => "Battle";

        internal async UniTask OnEnter(PlayerAccount playerAccount)
        {
            Debug.Log("BattleState OnEnter " + playerAccount.Account);
            var scene = await assetLoader.LoadSceneAsync(Name);
        }
    }
}

