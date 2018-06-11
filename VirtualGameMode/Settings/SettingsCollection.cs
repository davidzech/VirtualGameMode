using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VirtualGameMode.Models;

namespace VirtualGameMode.Settings
{
    [Serializable]
    public class SettingsCollection
    {
        public static SettingsCollection Default { get; } = FindOrCreate();

        private static SettingsCollection FindOrCreate()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var vgAppData = Path.Combine(appData, "VirtualGameMode");
            var settingsFile = Path.Combine(vgAppData, "settings.json");
            if (File.Exists(settingsFile))
            {                
                string jsonStr = File.ReadAllText(settingsFile);
                return JsonConvert.DeserializeObject<SettingsCollection>(jsonStr);
            }
            else
            {
                Directory.CreateDirectory(vgAppData);
                var settings = new SettingsCollection();
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

        public bool DisableAltF4 { get; set; }

        public KeyScope DisableAltF4Scope { get; set; }

        public bool DisableWinKey { get; set; }

        public KeyScope DisableWinKeyScope { get; set; }

        public bool DisableAltTab { get; set; }

        public KeyScope DisableAltTabScope { get; set; }

        public bool LaunchOnStartup { get; set; }

        public bool AutoGameMode { get; set; }

        public List<UserApplication> UserApplications { get; } = new List<UserApplication>();
    }
}
