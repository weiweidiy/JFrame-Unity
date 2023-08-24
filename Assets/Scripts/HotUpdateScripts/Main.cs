using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Adic;
using Adic.Container;

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

            container.Bind<YooAssetsLoader>().ToSingleton();
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

