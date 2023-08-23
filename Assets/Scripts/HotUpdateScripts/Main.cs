using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Adic;
using Adic.Container;

namespace JFrame.Game.HotUpdate
{
    /// <summary>
    /// 热更新入口，主要负责IOC容器绑定工作
    /// </summary>
    public class Main : ContextRoot
    {
        public static Main Ins;

        IInjectionContainer container;

        protected new void Awake()
        {
            if (Ins != null)
            {
                Debug.LogError("重复单例 :" + Ins.gameObject.name);
                Destroy(gameObject);
                return;
            }
            Ins = this;
            DontDestroyOnLoad(gameObject);

            base.Awake();
        }

        public override void SetupContainers()
        {
            //绑定ui管理器
            container = AddContainer<InjectionContainer>()
                .RegisterExtension<UnityBindingContainerExtension>();

            container.Bind<GameSM>().ToSingleton();
            container.Bind<GameManager>().ToSingleton();

            //.Bind<Transform>().ToGameObject("Cube")
            //.Bind<Test>().ToGameObject();


        }

        public override void Init()
        {
            var gameManager = container.Resolve<GameManager>();
            gameManager.Run();

            gameManager.ToGame(new PlayerAccount() { account = "1" });
            gameManager.ToMenu(true);

        }

    }
}

