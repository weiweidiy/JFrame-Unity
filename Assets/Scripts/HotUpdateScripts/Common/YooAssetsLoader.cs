using System;
using YooAsset;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace JFrame.Common
{

    public class YooAssetsLoader : IAssetsLoader
    {
        public async UniTask<Scene> LoadSceneAsync(string sceneName)
        {
            var handle = YooAssets.LoadSceneAsync(sceneName);
            await handle.ToUniTask();
            return handle.SceneObject;

        }


    }

}

