﻿using MahApps.Metro.Controls.Dialogs;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Input;
using VirtualGameMode.Annotations;
using VirtualGameMode.Commands;
using VirtualGameMode.Dialogs;
using VirtualGameMode.Models;
using VirtualGameMode.Settings;

namespace VirtualGameMode.ViewModels
{
    public sealed class ApplicationsViewModel : INotifyPropertyChanged
    {
        private static readonly BinaryFormatter bf = new BinaryFormatter();
        private readonly ObservableCollection<Models.UserApplication> _apps;
        public ObservableCollection<Models.UserApplication> Apps => _apps;

        public string NameFieldText { get; set; }
        public string ExeFieldText { get; set; }

        public bool CanAddItem => true;
        private readonly RelayCommand<object> _addItemCommand;
        public ICommand AddItemCommand => _addItemCommand;

        public void AddItem(object obj)
        {
            ShowAddDialog();
        }

        public async void ShowAddDialog()
        {
            var dialog = new AddApplicationDialog();
            var dataContext =
                new AddApplicationViewModel(o => { _coordinator.HideMetroDialogAsync(this, dialog); },
                    (model, application) =>
                    {
                        _coordinator.HideMetroDialogAsync(this, dialog);
                        _apps.Add(application);
                        SettingsCollection.Default.UserApplications.Add(application);
                    });
            dialog.DataContext = dataContext;
            await _coordinator.ShowMetroDialogAsync(this, dialog);
        }

        private readonly RelayCommand<UserApplication> _removeItemCommand;
        public ICommand RemoveItemCommand => _removeItemCommand;

        private void RemoveItem(UserApplication app)
        {
            _apps.Remove(app);
            SettingsCollection.Default.UserApplications.Remove(app);
        }

        private readonly IDialogCoordinator _coordinator;
        public ApplicationsViewModel(IDialogCoordinator coordinator)
        {
            _coordinator = coordinator;
            _addItemCommand = new RelayCommand<object>(AddItem, param => CanAddItem);
            _removeItemCommand = new RelayCommand<UserApplication>(RemoveItem);
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