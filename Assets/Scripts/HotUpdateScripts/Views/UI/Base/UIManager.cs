using Adic;
using Cysharp.Threading.Tasks;
using deVoid.UIFramework;
using JFrame.Common;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using static UnityEditor.Rendering.Universal.ShaderGUI.LitGUI;

namespace JFrame.Game.View
{
    /// <summary>
    /// UI管理器
    /// </summary>
    public class UIManager 
    {
        [Inject]
        IAssetsLoader assetsLoader;

        UIFrame uiFrame;

        /// <summary>
        /// 初始化
        /// </summary>
        public async UniTask Initialize()
        {
            var uiSetting = await assetsLoader.LoadAssetAsync<UISettings>("UISettings");
            uiFrame = uiSetting.CreateUIInstance();
            Camera.main.GetUniversalAdditionalCameraData().cameraStack.Add(uiFrame.UICamera);
        }

        /// <summary>
        /// 显示panel
        /// </summary>
        /// <param name="panelName"></param>
        public void ShowPanel(string panelName)
        {                
            uiFrame.ShowPanel(panelName);
        }

        public void ShowPanel<TArg>(string screenId, TArg properties) where TArg : IPanelProperties
        {
            uiFrame.ShowPanel(screenId, properties);
        }

        
    }
}
