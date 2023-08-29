using Adic;
using Cysharp.Threading.Tasks;
using JFrame.Common;
using JFrame.Game.Models;

namespace JFrame.Game.View
{
    public class MenuUIController : ViewController
    {
        [Inject]
        PlayerAccount playerAccount;

        [Inject]
        IAssetsLoader assetLoader;

        [Inject]
        UIManager uiManager;

        protected override UniTask DoShow()
        {
            uiManager.ShowPanel("BtnStart");

            playerAccount.Account = "Jichunwei";

            return UniTask.DelayFrame(1);
        }
    }
}
