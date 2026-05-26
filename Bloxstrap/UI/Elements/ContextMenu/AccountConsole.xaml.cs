using System.Windows;
using StarStrap.UI.ViewModels;

namespace StarStrap.UI.Elements.ContextMenu
{
    public partial class AccountManagerWindow
    {
        public AccountManagerWindow()
        {
            InitializeComponent();
            DataContext = new AccountBackupsViewModel();
        }
    }
}
