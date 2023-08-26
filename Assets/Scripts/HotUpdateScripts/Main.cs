using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Adic;
using Adic.Container;
using JFrame.Common;

namespace JFrame.Game.HotUpdate
{
    /// <summary>
    /// �ȸ�����ڣ���Ҫ����IOC�����󶨹���
    /// </summary>
    public class Main : ContextRoot
    {
        public static Main Ins;

        IInjectionContainer container;

        protected new void Awake()
        {
            if (Ins != null)
            {
                Debug.LogError("�ظ����� :" + Ins.gameObject.name);
                Destroy(gameObject);
                return;
            }
            Ins = this;
            DontDestroyOnLoad(gameObject);

            base.Awake();
        }

        public override void SetupContainers()
        {
            //��ui������
            container = AddContainer<InjectionContainer>()
                .RegisterExtension<UnityBindingContainerExtension>();

            //绑定通用逻辑
            container.Bind<IAssetsLoader>().ToSingleton<YooAssetsLoader>();

            //绑定业务逻辑
            container.Bind<SceneSM>().ToSingleton();
            container.Bind<SceneController>().ToSingleton();

        }

        public override void Init()
        {
            var gameManager = container.Resolve<SceneController>();
            gameManager.Run();

            gameManager.ToGame(new PlayerAccount() { account = "1" });
            //gameManager.ToMenu(true);

        }

    }
}

