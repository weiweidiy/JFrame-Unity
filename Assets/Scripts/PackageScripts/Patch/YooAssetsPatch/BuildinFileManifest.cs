using System.Collections.Generic;
using UnityEngine;

namespace JFrame.Game
{
    /// <summary>
    /// 内置资源清单
    /// </summary>
    public class BuildinFileManifest : ScriptableObject
    {
        public List<string> BuildinFiles = new List<string>();
    }
}