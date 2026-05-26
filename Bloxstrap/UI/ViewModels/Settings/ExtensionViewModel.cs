using System.ComponentModel;

namespace StarStrap.UI.ViewModels.Settings
{
    public class ExtensionViewModel : INotifyPropertyChanged
    {
        public bool FleasionEnabled
        {
            get => App.Settings.Prop.Fleasion;
            set
            {
                App.Settings.Prop.Fleasion = value;
                OnPropertyChanged(nameof(FleasionEnabled));
            }
        }

        public bool MemReductEnabled
        {
            get => App.Settings.Prop.MemReductEnabled;
            set
            {
                App.Settings.Prop.MemReductEnabled = value;
                OnPropertyChanged(nameof(MemReductEnabled));
            }
        }

        public bool OptimizeWindowsEnabled
        {
            get => App.Settings.Prop.OptimizeWindowsOnLaunch;
            set
            {
                App.Settings.Prop.OptimizeWindowsOnLaunch = value;
                OnPropertyChanged(nameof(OptimizeWindowsEnabled));
            }
        }

        public bool ClearTempFilesEnabled
        {
            get => App.Settings.Prop.ClearTempFilesEnabled;
            set
            {
                App.Settings.Prop.ClearTempFilesEnabled = value;
                OnPropertyChanged(nameof(ClearTempFilesEnabled));
            }
        }

        public bool HighCpuPriorityEnabled
        {
            get => App.Settings.Prop.HighCpuPriorityEnabled;
            set
            {
                App.Settings.Prop.HighCpuPriorityEnabled = value;
                OnPropertyChanged(nameof(HighCpuPriorityEnabled));
            }
        }

        public bool CancelXboxGameBarEnabled
        {
            get => App.Settings.Prop.CancelXboxGameBarEnabled;
            set
            {
                App.Settings.Prop.CancelXboxGameBarEnabled = value;
                OnPropertyChanged(nameof(CancelXboxGameBarEnabled));
            }
        }

        public bool CloseBackgroundAppsEnabled
        {
            get => App.Settings.Prop.CloseBackgroundAppsEnabled;
            set
            {
                App.Settings.Prop.CloseBackgroundAppsEnabled = value;
                OnPropertyChanged(nameof(CloseBackgroundAppsEnabled));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
