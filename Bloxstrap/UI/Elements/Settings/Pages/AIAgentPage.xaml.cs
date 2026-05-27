using StarStrap.UI.ViewModels.Settings;
using Wpf.Ui.Controls;

namespace StarStrap.UI.Elements.Settings.Pages
{
    public partial class AIAgentPage : UiPage
    {
        public AIAgentPage()
        {
            InitializeComponent();
            DataContext = new AIAgentViewModel();
        }
    }
}
