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

        bool isFirst = true;
        
        internal async UniTask OnEnter()
        {
            Debug.Assert(Owner != null, "owner is null");

            if (isFirst)
                return;

            var scene = await assetLoader.LoadSceneAsync(Name);

            isFirst = false;

        }

        internal UniTask OnExit()
        {
            return UniTask.DelayFrame(1);
        }
    }
}

