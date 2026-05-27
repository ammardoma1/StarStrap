using System.ComponentModel;
using System.Collections.Generic;
using StarStrap.Enums;

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

        public IReadOnlyDictionary<string, MemoryCleanerType> MemoryCleanerTypes => new Dictionary<string, MemoryCleanerType>
        {
            { "None", MemoryCleanerType.None },
            { "MemReduct", MemoryCleanerType.MemReduct },
            { "Windows Memory Cleaner", MemoryCleanerType.WindowsMemoryCleaner }
        };

        public MemoryCleanerType SelectedMemoryCleaner
        {
            get => App.Settings.Prop.SelectedMemoryCleaner;
            set
            {
                App.Settings.Prop.SelectedMemoryCleaner = value;
                OnPropertyChanged(nameof(SelectedMemoryCleaner));
            }
        }

        public bool WinhanceEnabled
        {
            get => App.Settings.Prop.WinhanceEnabled;
            set
            {
                App.Settings.Prop.WinhanceEnabled = value;
                OnPropertyChanged(nameof(WinhanceEnabled));
            }
        }

        public bool CompressRamEnabled
        {
            get => App.Settings.Prop.CompressRamEnabled;
            set
            {
                App.Settings.Prop.CompressRamEnabled = value;
                OnPropertyChanged(nameof(CompressRamEnabled));
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
