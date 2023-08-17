/****************************************************
    文件：TestAdic.cs
    作者：嵇春苇
    邮箱: 49595272@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/


using System;
using System.IO;

public class ScriptsInfoRecoder : UnityEditor.AssetModificationProcessor
{
    private static void OnWillCreateAsset(string path)
    {
        path = path.Replace(".meta", "");
        if (path.EndsWith(".cs"))
        {
            string str = File.ReadAllText(path);
            str = str/*.Replace("嵇春苇", Environment.UserName)*/
                    .Replace("#CreateTime#", string.Concat(DateTime.Now.Year, "/", DateTime.Now.Month, "/", DateTime.Now.Day, " ", DateTime.Now.Hour, ":", DateTime.Now.Minute, ":", DateTime.Now.Second))
                    .Replace("#NAMESPACEBEGIN#", "namespace hiplaygame");
                    
            File.WriteAllText(path, str);
        }
    }
}