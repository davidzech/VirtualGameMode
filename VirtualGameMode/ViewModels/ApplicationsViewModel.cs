using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VirtualGameMode.Annotations;
using VirtualGameMode.Models;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Input;
using VirtualGameMode.Commands;
using VirtualGameMode.Settings;

namespace VirtualGameMode.ViewModels
{
    public sealed class ApplicationsViewModel : INotifyPropertyChanged
    {
        private static readonly BinaryFormatter bf = new BinaryFormatter();
        private ObservableCollection<Models.UserApplication> _apps;
        public ObservableCollection<Models.UserApplication> Apps { get; }

        public string NameFieldText { get; set; }
        public string ExeFieldText { get; set; }

        public bool CanAddItem => true;
        private readonly RelayCommand<object> _addItemCommand;
        public ICommand AddItemCommand => _addItemCommand;

        public void AddItem(object obj)
        {
            var newItem = new UserApplication() {Name = NameFieldText, ExePath = ExeFieldText};
            _apps.Add(newItem);
            SettingsCollection.Default.UserApplications.Add(newItem);
        }

        public bool CanRemoveItem => false;
        private readonly RelayCommand<UserApplication> _removeItemCommand;
        public ICommand RemoveItemCommand => _removeItemCommand;

        public void RemoveItem(UserApplication app)
        {
            _apps.Remove(app);
            SettingsCollection.Default.UserApplications.Remove(app);
        }

        public ApplicationsViewModel()
        {
            _addItemCommand = new RelayCommand<object>(AddItem, param => CanAddItem);
            _apps = new ObservableCollection<UserApplication>(SettingsCollection.Default.UserApplications);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}