using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace JFrame.Common
{
    public interface IAssetsLoader
    {
        UniTask<Scene> LoadSceneAsync(string sceneName);
    }
}

