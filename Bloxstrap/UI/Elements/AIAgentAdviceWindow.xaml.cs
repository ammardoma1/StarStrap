using System.Windows;
using Wpf.Ui.Controls;

namespace StarStrap.UI.Elements
{
    public partial class AIAgentAdviceWindow : UiWindow
    {
        public AIAgentAdviceWindow(string advice)
        {
            InitializeComponent();
            AdviceTextBlock.Text = advice;
        }
    }
}
