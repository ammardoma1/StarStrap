using System.ComponentModel;
using Wpf.Ui.Controls;

namespace StarStrap.UI.Elements.Settings.Pages
{
    public partial class ExtensionPage : UiPage
    {
        public ExtensionPage()
        {
            DataContext = new StarStrap.UI.ViewModels.Settings.ExtensionViewModel();
            InitializeComponent();
        }
    }
}
