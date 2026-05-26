using System.Windows;
using StarStrap.UI.ViewModels;
using StarStrap.UI.ViewModels.ContextMenu;

namespace StarStrap.UI.Elements.ContextMenu
{
    public partial class RPCWindow
    {
        public RPCWindow()
        {
            InitializeComponent();
            DataContext = new RPCCustomizerViewModel();
        }
    }
}
