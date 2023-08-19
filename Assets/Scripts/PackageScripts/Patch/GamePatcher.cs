using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using YooAsset;

namespace JFrame.Game.Package
{

    public class GamePatcher : MonoSingleton<GamePatcher>
    {
        [SerializeField] EPlayMode playMode;

        [SerializeField] string hostDomainAddress = "http://127.0.0.1";

        /// <summary>
        /// �Ƿ���Ҫ�ȸ���⣬ֻ�����1��
        /// </summary>
        bool needPatch = true;

        /// <summary>
        /// �ȸ����Ԥ��������
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
        /// ��ʼ�ȸ�����
        /// </summary>
        public async void Patch()
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

        /// <summary>
        /// patch���
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