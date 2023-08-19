using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using YooAsset;

namespace JFrame.Game.Package
{
    public class YooAssetsPatchManager : IPatchManager
    {
        public event Action<int, long> onGetDownloadInfo;
        public event Action<bool> onPatchComplete;

        EPlayMode playMode;

        string hostDomainAddress;

        string DefaultPackageName = "DefaultPackage";

        ResourceDownloaderOperation downloader = null;

        public YooAssetsPatchManager(EPlayMode playMode, string hostDomainAddress)
        {
            this.playMode = playMode;
            this.hostDomainAddress = hostDomainAddress;
        }

        public void Clear()
        {
            downloader = null;
        }

        public async UniTask<bool> Initialize()
        {
            Clear();

            YooAssets.Initialize();
            YooAssets.SetOperationSystemMaxTimeSlice(30);

            // 创建默认的资源包
            string packageName = DefaultPackageName;
            var package = GetPackage(DefaultPackageName);
            if (package == null)
            {
                package = YooAssets.CreatePackage(packageName);
                YooAssets.SetDefaultPackage(package);
            }

            // 编辑器下的模拟模式
            InitializationOperation initializationOperation = null;
            if (playMode == EPlayMode.EditorSimulateMode)
            {
                var createParameters = new EditorSimulateModeParameters();
                createParameters.SimulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild(packageName);
                initializationOperation = package.InitializeAsync(createParameters);
            }

            // 单机运行模式
            if (playMode == EPlayMode.OfflinePlayMode)
            {
                var createParameters = new OfflinePlayModeParameters();
                createParameters.DecryptionServices = new GameDecryptionServices();
                initializationOperation = package.InitializeAsync(createParameters);
            }

            // 联机运行模式
            if (playMode == EPlayMode.HostPlayMode)
            {
                string defaultHostServer = GetHostServerURL(hostDomainAddress);
                string fallbackHostServer = GetHostServerURL(hostDomainAddress);
                var createParameters = new HostPlayModeParameters();
                createParameters.DecryptionServices = new GameDecryptionServices();
                createParameters.QueryServices = new GameQueryServices();
                createParameters.RemoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
                initializationOperation = package.InitializeAsync(createParameters);
            }

            // WebGL运行模式
            if (playMode == EPlayMode.WebPlayMode)
            {
                string defaultHostServer = GetHostServerURL(hostDomainAddress);
                string fallbackHostServer = GetHostServerURL(hostDomainAddress);
                var createParameters = new WebPlayModeParameters();
                createParameters.DecryptionServices = new GameDecryptionServices();
                createParameters.QueryServices = new GameQueryServices();
                createParameters.RemoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
                initializationOperation = package.InitializeAsync(createParameters);
            }

            await initializationOperation;

            //Check Init State
            if (package.InitializeStatus == EOperationStatus.Succeed)
            {
                // 运行补丁流程
                return true;
            }

            //初始化失败了
            Debug.LogError($"Init Error : {initializationOperation.Error}");

            return false;
        }

        public async void Run()
        {
            Debug.Log("开始补丁更新...");
            //创建patch相关ui

            //step 2 : 更新版本号
            Debug.Log("开始更新版本号...");
            var packageVersionOp = await UpdatePackageVersion();
            if (packageVersionOp.Status == EOperationStatus.Succeed)
            {
                Debug.Log("更新版本号成功！");
            }
            else
            {
                //失败
                Debug.LogError(packageVersionOp.Error);
                return;

            }

            //step 3: 更新清单文件
            Debug.Log("开始更新清单文件...");
            var manifestOp = await UpdatePackageManifest(packageVersionOp.PackageVersion);
            if (manifestOp.Status == EOperationStatus.Succeed)
            {
                //更新成功
                Debug.Log("更新清单文件成功！");
            }
            else
            {
                //更新失败
                Debug.LogError(manifestOp.Error);
                return;
            }

            //setp 4: 创建下载器
            downloader = CreateDownloader();

            if (downloader.TotalDownloadCount == 0)
            {
                //没有可下载的，结束
                onPatchComplete?.Invoke(true);
            }
            else
            {
                onGetDownloadInfo?.Invoke(downloader.TotalDownloadCount, downloader.TotalDownloadBytes);
            }

        }

        private void OnStartDownloadFileFunction(string fileName, long sizeBytes)
        {
            Debug.Log("OnStartDownloadFileFunction " + fileName);
        }

        private void OnDownloadOverFunction(bool isSucceed)
        {
            onPatchComplete?.Invoke(isSucceed);
        }

        private void OnDownloadProgressUpdateFunction(int totalDownloadCount, int currentDownloadCount, long totalDownloadBytes, long currentDownloadBytes)
        {
            //Debug.Log("OnDownloadProgressUpdateFunction " + fileName);
        }

        private void OnDownloadErrorFunction(string fileName, string error)
        {
            Debug.LogError("OnDownloadErrorFunction " + fileName + " / " + error);
        }

        /// <summary>
        /// 对于联机运行模式，在更新补丁清单之前，需要获取一个资源版本。
        /// 该资源版本可以通过YooAssets提供的接口来更新，也可以通过HTTP访问游戏服务器来获取。
        /// </summary>
        /// <returns></returns>
        private async UniTask<UpdatePackageVersionOperation> UpdatePackageVersion()
        {
            var package = GetPackage(DefaultPackageName);
            var operation = package.UpdatePackageVersionAsync();
            await operation;
            return operation;

        }

        /// <summary>
        /// 更新资源清单
        /// </summary>
        /// <returns></returns>
        private async UniTask<UpdatePackageManifestOperation> UpdatePackageManifest(string packageVersion)
        {
            // 更新成功后自动保存版本号，作为下次初始化的版本。
            // 也可以通过operation.SavePackageVersion()方法保存。
            bool savePackageVersion = true;
            var package = GetPackage(DefaultPackageName);
            var operation = package.UpdatePackageManifestAsync(packageVersion, savePackageVersion);
            await operation;
            return operation;
        }

        /// <summary>
        /// 创建下载器
        /// </summary>
        /// <returns></returns>
        private ResourceDownloaderOperation CreateDownloader()
        {
            Debug.Log("创建补丁下载器.");
            int downloadingMaxNum = 10;
            int failedTryAgain = 3;
            var downloader = YooAssets.CreateResourceDownloader(downloadingMaxNum, failedTryAgain);
            //await downloader;
            return downloader;
        }

        private async UniTask<ResourceDownloaderOperation> Download(ResourceDownloaderOperation op)
        {
            op.BeginDownload();
            await op;
            //    //检测下载结果
            //    if (downloaderOp.Status == EOperationStatus.Succeed)
            //    {
            //        //下载成功
            //    }
            //    else
            //    {
            //        //下载失败
            //    }
            return op;
        }


        /// <summary>
        /// 获取资源包
        /// </summary>
        /// <param name="packageName"></param>
        /// <returns></returns>
        private ResourcePackage GetPackage(string packageName, bool debug = false)
        {
            if (debug)
                return YooAssets.GetPackage(packageName);
            else
                return YooAssets.TryGetPackage(packageName);
        }

        /// <summary>
        /// 获取资源服务器地址
        /// </summary>
        private string GetHostServerURL(string hostDomainAddress)
        {
            //string hostServerIP = "http://10.0.2.2"; //安卓模拟器地址
            string hostServerIP = hostDomainAddress;
            string appVersion = "test";

#if UNITY_EDITOR
            if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.Android)
                return $"{hostServerIP}/CDN/Android/{appVersion}";
            else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.iOS)
                return $"{hostServerIP}/CDN/IPhone/{appVersion}";
            else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.WebGL)
                return $"{hostServerIP}/CDN/WebGL/{appVersion}";
            else
                return $"{hostServerIP}/CDN/PC/{appVersion}";
#else
		if (Application.platform == RuntimePlatform.Android)
			return $"{hostServerIP}/CDN/Android/{appVersion}";
		else if (Application.platform == RuntimePlatform.IPhonePlayer)
			return $"{hostServerIP}/CDN/IPhone/{appVersion}";
		else if (Application.platform == RuntimePlatform.WebGLPlayer)
			return $"{hostServerIP}/CDN/WebGL/{appVersion}";
		else
			return $"{hostServerIP}/CDN/PC/{appVersion}";
#endif
        }

        public async void Download()
        {
            if (downloader == null)
            {
                Debug.LogError("下载器为空");
                return;
            }
            //注册回调方法
            downloader.OnDownloadErrorCallback = OnDownloadErrorFunction;
            downloader.OnDownloadProgressCallback = OnDownloadProgressUpdateFunction;
            downloader.OnDownloadOverCallback = OnDownloadOverFunction;
            downloader.OnStartDownloadFileCallback = OnStartDownloadFileFunction;

            var op = await Download(downloader);
        }
    }
}
