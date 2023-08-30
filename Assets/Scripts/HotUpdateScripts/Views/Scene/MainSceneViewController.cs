using Adic;
using Adic.Container;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace JFrame.Game.View
{


    /// <summary>
    /// 主场景控制器,
    /// </summary>
    public class MainSceneViewController : ViewController
    {
        [Inject]
        IInjectionContainer container;

        [Inject]
        UIManager uiManager;

        protected override void Init()
        {
            container.Bind<MenuUIController>().ToSingleton();
        }

        protected override async UniTask DoShow()
        {
            Debug.Log("Main ui run 初始化main ui");
            await uiManager.Initialize();
            await container.Resolve<MenuUIController>().Show();
        }
    }
}
