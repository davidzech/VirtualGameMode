using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VirtualGameMode.Annotations;
using VirtualGameMode.Functions;

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
                    Console.WriteLine("Installing Hook");
                    GameModeHook.InstallHook();
                }
                else
                {
                    Console.WriteLine("Removing Hook");
                    GameModeHook.RemoveHook();
                }
                OnPropertyChanged();
            }
        }

        public MainWindowViewModel()
        {
            this.GameModeOn = Settings.Default.AutoGameMode;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
