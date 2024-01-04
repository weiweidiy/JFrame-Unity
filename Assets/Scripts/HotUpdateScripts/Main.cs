using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Adic;
using Adic.Container;
using JFrame.Common;
using JFrame.Game.Models;
using JFrame.Game.View;
using JFrame.Game.Commands;
//using UnityEditor.Search;

namespace JFrame.Game
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
                .RegisterExtension<UnityBindingContainerExtension>()
                .RegisterExtension<CommanderContainerExtension>();



            //绑定通用逻辑
            container.Bind<IAssetsLoader>().ToSingleton<YooAssetsLoader>();
            container.Bind<ITransitionProvider>().ToSingleton<SMTransitionProvider>();
            container.Bind<UIManager>().ToSingleton();

            //绑定模型
            container.Bind<PlayerAccount>().ToSingleton();

            //绑定系统


            //绑定视图controller
            container.Bind<SceneSM>().ToSingleton();
            container.Bind<SceneController>().ToSingleton();
            container.Bind<SceneTransitionController>().ToSingleton();
            



            //绑定命令
            container.RegisterCommand<RunGame>();
            container.RegisterCommand<StartBattle>();


        }

        public override void Init()
        {
            var dispatcher = container.GetCommandDispatcher();
            dispatcher.Dispatch<RunGame>();

            //sceneController.SwitchToBattle(new PlayerAccount() { account = "1" });
            //gameManager.ToMenu(true);

        }

    }
}

