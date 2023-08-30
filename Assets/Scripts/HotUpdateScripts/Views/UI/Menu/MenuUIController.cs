using Adic;
using Adic.Container;
using Cysharp.Threading.Tasks;
using JFrame.Common;
using JFrame.Game.Commands;
using JFrame.Game.Models;
using System;
using UnityEngine;

namespace JFrame.Game.View
{
    public class MenuUIController : ViewController
    {
        [Inject]
        PlayerAccount playerAccount;

        [Inject]
        UIManager uiManager;

        [Inject]
        IInjectionContainer container;

  
        protected override void Init()
        {           
            container.Bind<MenuProperties>().ToSelf();
        }

        protected override UniTask DoShow()
        {
            var properties = container.Resolve<MenuProperties>();
            uiManager.ShowPanel("BtnStart", properties);
            properties.onClicked += OnClicked;

            playerAccount.Account = "Jichunwei";

            return UniTask.DelayFrame(1);
        }


        private void OnClicked()
        {
            var dispatcher = container.GetCommandDispatcher();
            dispatcher.Dispatch<StartBattle>();
        }
    }
}
