using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JFrame.Common
{
    public interface IAssetsLoader
    {
        UniTask<Scene> LoadSceneAsync(string sceneName);

        UniTask<GameObject> InstantiateAsync(string location);
    }
}

