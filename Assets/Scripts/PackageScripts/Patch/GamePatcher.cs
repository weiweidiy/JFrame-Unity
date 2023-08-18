using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using YooAsset;

public class GamePatcher : MonoBehaviour
{
    [SerializeField] EPlayMode playMode;

    [SerializeField] string hostDomainAddress = "http://127.0.0.1";

    // Start is called before the first frame update
    async void Start()
    {
        //��ʼ��YooAssets
        var patchManager = new YooAssetsPatchManager(playMode, hostDomainAddress);
        patchManager.onPatchComplete += PatchManager_onPatchComplete;

        var patchMediator = new PatchMediator(patchManager);

        bool result = await patchManager.Initialize();

        if (result == true)
        {
            //��ʼ��������
            patchManager.Run();
        }
        else
        {
            Debug.LogError("YooAsset��ʼ��ʧ�ܣ�");
        }
    }

    private async void PatchManager_onPatchComplete(bool isSucceed)
    {

#if !UNITY_EDITOR
        var dllManager = new DllManager();
        dllManager.LoadDLL();
#endif

        var _Handle = YooAssets.LoadAssetAsync<GameObject>("Main");
        await _Handle.ToUniTask();
        _Handle.InstantiateSync();
    }
}
