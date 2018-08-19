using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VirtualGameMode.Models;

namespace VirtualGameMode
{
    [Serializable]
    public sealed class Settings : INotifyPropertyChanged
    {
        public static Settings Default { get; } = FindOrCreate();

        private static Settings FindOrCreate()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var vgAppData = Path.Combine(appData, "VirtualGameMode");
            var settingsFile = Path.Combine(vgAppData, "settings.json");
            if (File.Exists(settingsFile))
            {                
                string jsonStr = File.ReadAllText(settingsFile);
                return JsonConvert.DeserializeObject<VirtualGameMode.Settings>(jsonStr);
            }
            else
            {
                Directory.CreateDirectory(vgAppData);
                var settings = new Settings();
                settings.Save();
                return settings;
            }
        }

        public void Save()
        {            
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var settingsFile = Path.Combine(appData, "VirtualGameMode", "settings.json");
            File.WriteAllText(settingsFile, JsonConvert.SerializeObject(this));
        }

        private bool _disableAltF4;
        public bool DisableAltF4
        {
            get => _disableAltF4;
            set
            {
                if (_disableAltF4 != value)
                {
                    _disableAltF4 = value;
                    OnPropertyChanged();
                }                
            }
        }

        private KeyScope _disableAltF4Scope;

        public KeyScope DisableAltF4Scope
        {
            get => _disableAltF4Scope;
            set
            {
                if (_disableAltF4Scope != value)
                {
                    _disableAltF4Scope = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _disableWinKey;

        public bool DisableWinKey
        {
            get => _disableWinKey;
            set
            {
                if (_disableWinKey != value)
                {
                    _disableWinKey = value;
                    OnPropertyChanged();
                }
            }
        }

        private KeyScope _disableWinKeyScope;

        public KeyScope DisableWinKeyScope
        {
            get => _disableWinKeyScope;
            set
            {
                if (_disableWinKeyScope != value)
                {
                    _disableWinKeyScope = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _disableAltTab;

        public bool DisableAltTab
        {
            get => _disableAltTab;
            set
            {
                if(_disableAltTab != value)
                {
                    _disableAltTab = value;
                    OnPropertyChanged();
                }
            }
        }

        private KeyScope _disableAltTabScope;

        public KeyScope DisableAltTabScope
        {
            get => _disableAltTabScope;
            set
            {
                if (_disableAltTabScope != value)
                {
                    _disableAltTabScope = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _disableAltSpace;

        public bool DisableAltSpace
        {
            get => _disableAltSpace;
            set
            {
                if (_disableAltSpace != value)
                {
                    _disableAltSpace = value;
                    OnPropertyChanged();
                }
            }
        }

        private KeyScope _disableAltSpaceScope;

        public KeyScope DisableAltSpaceScope
        {
            get => _disableAltSpaceScope;
            set
            {
                if (_disableAltSpaceScope != value)
                {
                    _disableAltSpaceScope = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _launchOnStartup;

        public bool LaunchOnStartup
        {
            get => _launchOnStartup;
            set
            {
                if (_launchOnStartup != value)
                {
                    _launchOnStartup = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _startMinimized;

        public bool StartMinimized
        {
            get => _startMinimized;
            set
            {
                if (_startMinimized != value)
                {
                    _startMinimized = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _autoGameMode;

        public bool AutoGameMode
        {
            get => _autoGameMode;
            set
            {
                if (_autoGameMode != value)
                {
                    _autoGameMode = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<UserApplication> UserApplications { get; } = new ObservableCollection<UserApplication>();

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
