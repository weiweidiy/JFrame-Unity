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
        //初始化YooAssets
        var patchManager = new YooAssetsPatchManager(playMode, hostDomainAddress);
        var patchMediator = new PatchMediator(patchManager);

        bool result = await patchManager.Initialize();

        if (result == true)
        {
            //开始更新流程
            patchManager.Run();
        }
        else
        {
            Debug.LogError("YooAsset初始化失败！");
        }
    }

}
