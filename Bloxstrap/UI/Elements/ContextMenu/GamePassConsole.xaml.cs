using StarStrap.Integrations;
using StarStrap.UI.Elements.Base;
using StarStrap.UI.ViewModels.ContextMenu;

namespace StarStrap.UI.Elements.ContextMenu
{
    public partial class GamePassConsole
    {
        public GamePassConsole(long userId)
        {
            InitializeComponent();
            var vm = new GamePassConsoleViewModel();
            DataContext = vm;
            vm.LoadGamePassesCommand.Execute(userId);
        }
    }
}
