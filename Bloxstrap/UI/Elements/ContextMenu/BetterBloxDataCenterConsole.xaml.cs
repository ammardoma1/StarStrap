using StarStrap.Integrations;
using StarStrap.UI.Elements.Base;
using StarStrap.UI.ViewModels.ContextMenu;

namespace StarStrap.UI.Elements.ContextMenu
{
    public partial class BetterBloxDataCenterConsole
    {
        public BetterBloxDataCenterConsole()
        {
            InitializeComponent();
            var vm = new BetterBloxDataCenterConsoleViewModel();
            DataContext = vm;
        }
    }
}
