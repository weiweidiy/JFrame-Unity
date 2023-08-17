using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders;


namespace HiplayGame
{

    /// <summary>
    /// * ��Դ������
    /// *
    /// * ���� ����/����/�ͷ� ��Ϸһ���õ�����Դ
    /// *
    /// *
    /// </summary>
    public interface IAssetLoader
    {
        /// <summary>
        /// ����һ����Դ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        UniTask<T> LoadAssetAsync<T>(string resourceName) where T : class;

        /// <summary>
        /// ���س����� to do: ���Ե�����ֳ�������
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        UniTask<SceneInstance> LoadSceneAsync(string sceneName);

        /// <summary>
        /// ͬ�����س���
        /// </summary>
        /// <param name="secenName"></param>
        /// <returns></returns>
        SceneInstance LoadScene(string secenName);

        /// <summary>
        /// ʵ������Ϸ���� to do: ���Ե�����ֳ�GameObjectManager
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="parent"></param>
        /// <param name="instantiateInWorldSpace"></param>
        /// <returns></returns>
        UniTask<GameObject> InstantiateAsync(string resourceName, Transform parent = null, bool instantiateInWorldSpace = false, Action<GameObject> complete = null);
        UniTask<GameObject> InstantiateAsync(string resourceName, Vector3 position, Quaternion quaternion, Transform parent = null, Action<GameObject> complete = null);

        /// <summary>
        /// ͬ��ʵ������Ϸ����
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="parent"></param>
        /// <param name="instantiateInWorldSpace"></param>
        /// <returns></returns>
        GameObject Instantiate(string resourceName, Transform parent = null, bool instantiateInWorldSpace = false);

        /// <summary>
        /// �ͷ���Դ
        /// </summary>
        /// <param name="resourceName"></param>
        void Release(string resourceName);
    }

}
