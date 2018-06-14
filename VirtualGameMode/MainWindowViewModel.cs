using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VirtualGameMode.Annotations;
using VirtualGameMode.Functions;
using VirtualGameMode.Settings;

namespace VirtualGameMode.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {

        private bool _gameModeOn;
        public bool GameModeOn
        {
            get => _gameModeOn;
            set
            {
                _gameModeOn = value;
                if (value == true)
                {
                    //this.GameModeToggle.Content = "Game Mode is On";
                    //this.GameModeToggle.Foreground = (Brush) FindResource("AccentColorBrush");
                    //this.GameModeToggle.IsChecked = true;
                    GameModeHook.InstallHook();
                }
                else
                {
                    //this.GameModeToggle.Content = "Game Mode is Off";
                    //this.GameModeToggle.Foreground = (Brush) FindResource("TextBrush");
                    //this.GameModeToggle.IsChecked = false;
                    GameModeHook.RemoveHook();
                }
                OnPropertyChanged();
            }
        }

        public MainWindowViewModel()
        {
            this.GameModeOn = SettingsCollection.Default.AutoGameMode;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
