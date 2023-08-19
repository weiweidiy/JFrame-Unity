using Cysharp.Threading.Tasks;
using System;

namespace JFrame.Game.Package
{
    public interface IPatchManager
    {
        /// <summary>
        /// 获取到有下载内容时候的事件
        /// </summary>
        event Action<int, long> onGetDownloadInfo;

        /// <summary>
        /// 更新流程完成事件
        /// </summary>
        event Action<bool> onPatchComplete;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        UniTask<bool> Initialize();

        /// <summary>
        /// 启动下载流程
        /// </summary>
        void Run();

        /// <summary>
        /// 开始下载
        /// </summary>
        void Download();
    }
}