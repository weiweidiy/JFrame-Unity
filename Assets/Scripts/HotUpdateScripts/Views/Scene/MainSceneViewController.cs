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
        SceneController sceneController;

        [Inject]
        MenuUIController menuController;

        [Inject]
        void Init()
        {
            container.Bind<MenuUIController>().ToSingleton();
            sceneController.onSceneEnter += OnSceneEnter;    
        }

        private async void OnSceneEnter(string scene)
        {
            if (!scene.Equals("Main"))
                return;

            await Show();
        }

        protected override async UniTask DoShow()
        {
            Debug.Log("Main ui run 初始化main ui");
            await menuController.Show();
        }
    }
}
