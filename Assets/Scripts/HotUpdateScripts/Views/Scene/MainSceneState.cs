using System;
using Adic;
using Cysharp.Threading.Tasks;
using JFrame.Common;
using UnityEngine;

namespace JFrame.Game.View
{


    public class MainSceneState : SceneBaseState
    {
        public override string Name => "Main";

        [Inject]
        IAssetsLoader assetLoader;
        
        internal async UniTask OnEnter(bool isRestart)
        {
            Debug.Log("MainState OnEnter " + isRestart);
            Debug.Assert(Owner != null, "owner is null");
            if(isRestart)
            {
                var scene = await assetLoader.LoadSceneAsync(Name);
            }
            else
            {
                //Debug.Log("加载menu");
                await UniTask.DelayFrame(1);
            }
                
        }

        internal UniTask OnExit()
        {
            return UniTask.DelayFrame(1);
        }
    }
}

