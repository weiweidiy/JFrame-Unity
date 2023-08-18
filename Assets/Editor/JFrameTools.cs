
using System.IO;
//using Framework.Extension;
using HybridCLR.Editor;
using UnityEditor;
using UnityEngine;
using YooAsset;

public class JFrameTools
{
    [MenuItem("JFrameTools/ClearSave")]
    public static void ClearSave()
    {
        PlayerPrefs.DeleteAll();
    }
    
    /// <summary>
    /// 拷贝补充元数据AOT DLL到热更新目录里
    /// </summary>
    [MenuItem("JFrameTools/CopyAOTDLlToHotFixDir")]
    public static void CopyAOTAssembliesToStreamingAssets()
    {
        var target = EditorUserBuildSettings.activeBuildTarget;
        string aotAssembliesSrcDir = SettingsUtil.GetAssembliesPostIl2CppStripDir(target);
        string aotAssembliesDstDir = Application.dataPath + "/" + "Downloads/HotUpdateDll";

        foreach (var dll in SettingsUtil.HybridCLRSettings.patchAOTAssemblies)
        {
            string srcDllPath = $"{aotAssembliesSrcDir}/{dll}";
            if (!File.Exists(srcDllPath))
            {
                Debug.LogError($"ab中添加AOT补充元数据dll:{srcDllPath} 时发生错误,文件不存在。裁剪后的AOT dll在BuildPlayer时才能生成，因此需要你先构建一次游戏App后再打包。");
                continue;
            }
            string dllBytesPath = $"{aotAssembliesDstDir}/{dll}.bytes";
            File.Copy(srcDllPath, dllBytesPath, true);
            Debug.Log($"[CopyAOTAssembliesToStreamingAssets] copy AOT dll {srcDllPath} -> {dllBytesPath}");
        }

        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 拷贝热更新DLL 到热更新目录
    /// </summary>
    [MenuItem("JFrameTools/CopyHotFixDLlToHotFixDir")]
    public static void CopyHotUpdateAssembliesToStreamingAssets()
    {
        var target = EditorUserBuildSettings.activeBuildTarget;

        string hotfixDllSrcDir = SettingsUtil.GetHotUpdateDllsOutputDirByTarget(target);
        string hotfixAssembliesDstDir = Application.dataPath + "/" + "Downloads/HotUpdateDll";
        foreach (var dll in SettingsUtil.HotUpdateAssemblyNamesExcludePreserved)
        {
            string dllPath = $"{hotfixDllSrcDir}/{dll}.dll";
            string dllBytesPath = $"{hotfixAssembliesDstDir}/{dll}.dll.bytes";
            File.Copy(dllPath, dllBytesPath, true);
            Debug.Log($"[CopyHotUpdateAssembliesToStreamingAssets] copy hotfix dll {dllPath} -> {dllBytesPath}");
        }

        AssetDatabase.Refresh();
    }
}
