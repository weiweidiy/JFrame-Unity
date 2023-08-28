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

        protected override async UniTask DoShow()
        {
            var menu = await assetLoader.InstantiateAsync("Menu");
            playerAccount.Account = "Jichunwei";
        }


    }
}
