using deVoid.UIFramework;
using UnityEngine.UI;

namespace JFrame.Game.View
{
    public class MenuView : APanelController<MenuProperties>
    {
        Button btn;
        protected override void OnPropertiesSet()
        {
            base.OnPropertiesSet();

            btn = GetComponent<Button>();
            btn.onClick.AddListener(() => {

                Properties.onClicked?.Invoke();
            
            });
        }
    }
}
