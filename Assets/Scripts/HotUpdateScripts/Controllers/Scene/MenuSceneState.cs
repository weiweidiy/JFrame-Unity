using Adic;
using Cysharp.Threading.Tasks;
using JFrame.Common;
using UnityEngine;

namespace JFrame.Game.HotUpdate
{


    public class MenuSceneState : SceneBaseState
    {
        public override string Name => "Menu";

        [Inject]
        IAssetsLoader assetLoader;
        
        internal async UniTask OnEnter(bool isRestart)
        {
            Debug.Log("MenuState OnEnter " + isRestart);
            Debug.Assert(Owner != null, "owner is null");
            if(isRestart)
            {
                var scene = await assetLoader.LoadSceneAsync(Name);
            }
            else
            {
                Debug.Log("加载menu");
                await UniTask.DelayFrame(1);
            }
                
        }
    }
}

