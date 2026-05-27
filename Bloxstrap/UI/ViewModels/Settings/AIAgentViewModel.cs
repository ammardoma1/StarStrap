using System.Collections.Generic;
using StarStrap.Enums;
using StarStrap.Models;

namespace StarStrap.UI.ViewModels.Settings
{
    public class AIAgentViewModel : NotifyPropertyChangedViewModel
    {
        public bool AIAgentEnabled
        {
            get => App.Settings.Prop.AIAgentEnabled;
            set
            {
                App.Settings.Prop.AIAgentEnabled = value;
                OnPropertyChanged(nameof(AIAgentEnabled));
            }
        }

        public IEnumerable<AIAgentProvider> Providers => System.Enum.GetValues(typeof(AIAgentProvider)).Cast<AIAgentProvider>();

        public AIAgentProvider AIAgentProvider
        {
            get => App.Settings.Prop.AIAgentProvider;
            set
            {
                App.Settings.Prop.AIAgentProvider = value;
                OnPropertyChanged(nameof(AIAgentProvider));
            }
        }

        public string AIAgentApiKey
        {
            get => App.Settings.Prop.AIAgentApiKey;
            set
            {
                App.Settings.Prop.AIAgentApiKey = value;
                OnPropertyChanged(nameof(AIAgentApiKey));
            }
        }

        public string AIAgentSystemPrompt
        {
            get => App.Settings.Prop.AIAgentSystemPrompt;
            set
            {
                App.Settings.Prop.AIAgentSystemPrompt = value;
                OnPropertyChanged(nameof(AIAgentSystemPrompt));
            }
        }

        public string AIAgentHotkey
        {
            get => App.Settings.Prop.AIAgentHotkey;
            set
            {
                App.Settings.Prop.AIAgentHotkey = value;
                OnPropertyChanged(nameof(AIAgentHotkey));
            }
        }
    }
}
