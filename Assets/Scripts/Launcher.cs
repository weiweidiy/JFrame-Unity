using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using YooAsset;
//using DG.Tween;

public class Launcher : MonoBehaviour
{
    [SerializeField] EPlayMode playMode;

    // Start is called before the first frame update
    async void Start()
    {
        //初始化YooAssets
        var initYooAssets = InitYooAssets();

        //播放splash
        var splashTask = PlaySplash();
        
        //等待2个任务完成
        await UniTask.WhenAll(splashTask, initYooAssets);

        //切换场景
        SceneManager.LoadScene("Main");
    }

    /// <summary>
    /// 播放splash动画
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private UniTask PlaySplash()
    {
        return UniTask.Delay(5000);
    }

    /// <summary>
    /// 初始化YooAssets    
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private UniTask InitYooAssets()
    {
        YooAssets.Initialize();

        // 创建默认的资源包
        string packageName = "DefaultPackage";
        var package = YooAssets.TryGetPackage(packageName);
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
            string defaultHostServer = GetHostServerURL();
            string fallbackHostServer = GetHostServerURL();
            var createParameters = new HostPlayModeParameters();
            createParameters.DecryptionServices = new GameDecryptionServices();
            createParameters.QueryServices = new GameQueryServices();
            createParameters.RemoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
            initializationOperation = package.InitializeAsync(createParameters);
        }

        // WebGL运行模式
        if (playMode == EPlayMode.WebPlayMode)
        {
            string defaultHostServer = GetHostServerURL();
            string fallbackHostServer = GetHostServerURL();
            var createParameters = new WebPlayModeParameters();
            createParameters.DecryptionServices = new GameDecryptionServices();
            createParameters.QueryServices = new GameQueryServices();
            createParameters.RemoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
            initializationOperation = package.InitializeAsync(createParameters);
        }

        return initializationOperation.ToUniTask();
    }

    /// <summary>
	/// 远端资源地址查询服务类
	/// </summary>
	private class RemoteServices : IRemoteServices
    {
        private readonly string _defaultHostServer;
        private readonly string _fallbackHostServer;

        public RemoteServices(string defaultHostServer, string fallbackHostServer)
        {
            _defaultHostServer = defaultHostServer;
            _fallbackHostServer = fallbackHostServer;
        }
        string IRemoteServices.GetRemoteMainURL(string fileName)
        {
            return $"{_defaultHostServer}/{fileName}";
        }
        string IRemoteServices.GetRemoteFallbackURL(string fileName)
        {
            return $"{_fallbackHostServer}/{fileName}";
        }
    }

    /// <summary>
    /// 资源文件解密服务类
    /// </summary>
    private class GameDecryptionServices : IDecryptionServices
    {
        public ulong LoadFromFileOffset(DecryptFileInfo fileInfo)
        {
            return 32;
        }

        public byte[] LoadFromMemory(DecryptFileInfo fileInfo)
        {
            throw new NotImplementedException();
        }

        public Stream LoadFromStream(DecryptFileInfo fileInfo)
        {
            BundleStream bundleStream = new BundleStream(fileInfo.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return bundleStream;
        }

        public uint GetManagedReadBufferSize()
        {
            return 1024;
        }
    }

    /// <summary>
	/// 获取资源服务器地址
	/// </summary>
	private string GetHostServerURL()
    {
        //string hostServerIP = "http://10.0.2.2"; //安卓模拟器地址
        string hostServerIP = "http://127.0.0.1";
        string appVersion = "v1.0";

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


    /// <summary>
    /// 资源文件查询服务类
    /// </summary>
    public class GameQueryServices : IQueryServices
    {
        public DeliveryFileInfo GetDeliveryFileInfo(string packageName, string fileName)
        {
            throw new System.NotImplementedException();
        }

        public bool QueryDeliveryFiles(string packageName, string fileName)
        {
            return false;
        }

        public bool QueryStreamingAssets(string packageName, string fileName)
        {
            // 注意：fileName包含文件格式
            return StreamingAssetsHelper.FileExists(packageName, fileName);
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// StreamingAssets目录下资源查询帮助类
    /// </summary>
    public sealed class StreamingAssetsHelper
    {
        public static void Init() { }
        public static bool FileExists(string packageName, string fileName)
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, StreamingAssetsDefine.RootFolderName, packageName, fileName);
            return File.Exists(filePath);
        }
    }
#else
/// <summary>
/// StreamingAssets目录下资源查询帮助类
/// </summary>
public sealed class StreamingAssetsHelper
{
	private static bool _isInit = false;
	private static readonly HashSet<string> _cacheData = new HashSet<string>();

	/// <summary>
	/// 初始化
	/// </summary>
	public static void Init()
	{
		if (_isInit == false)
		{
			_isInit = true;
			var manifest = Resources.Load<BuildinFileManifest>("BuildinFileManifest");
			if (manifest != null)
			{
				foreach (string fileName in manifest.BuildinFiles)
				{
					_cacheData.Add(fileName);
				}
			}
		}
	}

	/// <summary>
	/// 内置文件查询方法
	/// </summary>
	public static bool FileExists(string packageName, string fileName)
	{
		if (_isInit == false)
			Init();
		return _cacheData.Contains(fileName);
	}
}
#endif


#if UNITY_EDITOR
    internal class PreprocessBuild : UnityEditor.Build.IPreprocessBuildWithReport
    {
        public int callbackOrder { get { return 0; } }

        /// <summary>
        /// 在构建应用程序前处理
        /// </summary>
        public void OnPreprocessBuild(UnityEditor.Build.Reporting.BuildReport report)
        {
            string saveFilePath = "Assets/Resources/BuildinFileManifest.asset";
            if (File.Exists(saveFilePath))
                File.Delete(saveFilePath);

            string folderPath = $"{Application.dataPath}/StreamingAssets/{StreamingAssetsDefine.RootFolderName}";
            DirectoryInfo root = new DirectoryInfo(folderPath);
            if (root.Exists == false)
            {
                Debug.Log($"没有发现YooAsset内置目录 : {folderPath}");
                return;
            }

            var manifest = ScriptableObject.CreateInstance<BuildinFileManifest>();
            FileInfo[] files = root.GetFiles("*", SearchOption.AllDirectories);
            foreach (var fileInfo in files)
            {
                if (fileInfo.Extension == ".meta")
                    continue;
                if (fileInfo.Name.StartsWith("PackageManifest_"))
                    continue;
                manifest.BuildinFiles.Add(fileInfo.Name);
            }

            if (Directory.Exists("Assets/Resources") == false)
                Directory.CreateDirectory("Assets/Resources");
            UnityEditor.AssetDatabase.CreateAsset(manifest, saveFilePath);
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
            Debug.Log($"一共{manifest.BuildinFiles.Count}个内置文件，内置资源清单保存成功 : {saveFilePath}");
        }
    }
#endif


    public class StreamingAssetsDefine
    {
        public const string RootFolderName = "yoo";
    }

    /// <summary>
    /// 内置资源清单
    /// </summary>
    public class BuildinFileManifest : ScriptableObject
    {
        public List<string> BuildinFiles = new List<string>();
    }

    public class BundleStream : FileStream
    {
        public const byte KEY = 64;

        public BundleStream(string path, FileMode mode, FileAccess access, FileShare share) : base(path, mode, access, share)
        {
        }
        public BundleStream(string path, FileMode mode) : base(path, mode)
        {
        }

        public override int Read(byte[] array, int offset, int count)
        {
            var index = base.Read(array, offset, count);
            for (int i = 0; i < array.Length; i++)
            {
                array[i] ^= KEY;
            }
            return index;
        }
    }
}
