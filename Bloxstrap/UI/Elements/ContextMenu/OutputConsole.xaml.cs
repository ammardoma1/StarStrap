using StarStrap.Integrations;
using StarStrap.UI.ViewModels.ContextMenu;

namespace StarStrap.UI.Elements.ContextMenu
{
    public partial class OutputConsole
    {
        public OutputConsole(ActivityWatcher watcher)
        {
            var viewModel = new OutputConsoleViewModel(watcher);

            viewModel.RequestCloseEvent += (_, _) => Close();

            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
