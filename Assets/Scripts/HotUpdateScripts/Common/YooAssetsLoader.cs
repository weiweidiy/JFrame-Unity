using System;
using YooAsset;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace JFrame.Common
{

    public class YooAssetsLoader : IAssetsLoader
    {
        public async UniTask<GameObject> Instantiate(string location)
        {
            var handle = YooAssets.LoadAssetAsync<GameObject>(location);
            await handle.ToUniTask();
            var h = handle.InstantiateAsync();
            await h;
            return h.Result;
        }

        public async UniTask<Scene> LoadSceneAsync(string sceneName)
        {
            var handle = YooAssets.LoadSceneAsync(sceneName);
            await handle.ToUniTask();
            return handle.SceneObject;

        }


    }

}

