using System.Windows.Controls;
using StarStrap.UI.ViewModels.Pages;

namespace StarStrap.UI.Elements.Settings.Pages
{
    public partial class HistoryPage : Page
    {
        private readonly HistoryPageViewModel _viewModel;
        public HistoryPage()
        {
            InitializeComponent();
            _viewModel = new HistoryPageViewModel();
            DataContext = _viewModel;
        }
    }
}
