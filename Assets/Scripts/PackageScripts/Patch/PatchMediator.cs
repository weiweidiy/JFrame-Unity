
//using DG.Tween;

using UnityEngine;

namespace JFrame.Game
{
    public class PatchMediator
    {
        IPatchManager patchManager;
        public PatchMediator(IPatchManager patchManager)
        {
            this.patchManager = patchManager;
            this.patchManager.onGetDownloadInfo += PatchManager_onPullDownloadInfo;
            this.patchManager.onPatchComplete += PatchManager_onPatchComplete;
        }

        private void PatchManager_onPatchComplete(bool isSucceed)
        {
            Debug.Log("PatchMediator : 更新完成了");
        }

        private void PatchManager_onPullDownloadInfo(int count, long size)
        {
            Debug.Log("PatchMediator : 检测到有下载内容 " + size);
            patchManager.Download();
        }
    }
}