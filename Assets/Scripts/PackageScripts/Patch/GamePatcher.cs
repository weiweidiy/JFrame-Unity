using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using YooAsset;

namespace JFrame.Game
{

    public class GamePatcher : MonoSingleton<GamePatcher>
    {
        [SerializeField] EPlayMode playMode;

        [SerializeField] string hostDomainAddress = "http://127.0.0.1";

        /// <summary>
        /// 是否需要热更检测，只会进行1次
        /// </summary>
        bool needPatch = true;

        /// <summary>
        /// 热更入口预制体名字
        /// </summary>
        [SerializeField] string entryName = "Main";

        protected override void Awake()
        {
            base.Awake();

            DontDestroyOnLoad(gameObject);
        }

        // Start is called before the first frame update
        void Start()
        {
            if(needPatch)
            {
                Patch();
                needPatch = false;
            }
            else
            {
                LoadEntry(entryName);
            }
                
        }

        /// <summary>
        /// 开始热更流程
        /// </summary>
        public async void Patch()
        {
            //初始化YooAssets
            var patchManager = new YooAssetsPatchManager(playMode, hostDomainAddress);
            patchManager.onPatchComplete += PatchManager_onPatchComplete;

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

        /// <summary>
        /// patch完成
        /// </summary>
        /// <param name="isSucceed"></param>
        private void PatchManager_onPatchComplete(bool isSucceed)
        {

#if !UNITY_EDITOR
        var dllManager = new DllManager();
        dllManager.LoadDLL();
#endif

            LoadEntry(entryName);
        }


        async void LoadEntry(string entryName)
        {
            var _Handle = YooAssets.LoadAssetAsync<GameObject>(entryName);
            await _Handle.ToUniTask();
            _Handle.InstantiateSync();
        }

    }
}