#if UNITY_EDITOR
using System.IO;
using UnityEngine;

internal class PreprocessBuild : UnityEditor.Build.IPreprocessBuildWithReport
{
    public int callbackOrder { get { return 0; } }

    /// <summary>
    /// �ڹ���Ӧ�ó���ǰ����
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
            Debug.Log($"û�з���YooAsset����Ŀ¼ : {folderPath}");
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
        Debug.Log($"һ��{manifest.BuildinFiles.Count}�������ļ���������Դ�嵥����ɹ� : {saveFilePath}");
    }
}

#endif