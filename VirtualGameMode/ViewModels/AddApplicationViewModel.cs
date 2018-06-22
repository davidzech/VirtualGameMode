using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VirtualGameMode.Annotations;
using VirtualGameMode.Commands;
using VirtualGameMode.Models;
using VirtualGameMode.Utilities;

namespace VirtualGameMode.ViewModels
{
    public class AddApplicationViewModel : INotifyPropertyChanged
    {
        public AddApplicationViewModel(Action<AddApplicationViewModel> cancelAction, Action<AddApplicationViewModel, UserApplication> okAction)
        {
            _cancelCommand = new RelayCommand<object>(o => cancelAction(this));
            _okCommand = new RelayCommand<UserApplication>(o => okAction(this, o), o => CanAdd);
            FindActiveWindows();
        }

        private readonly RelayCommand<object> _cancelCommand;
        public ICommand CancelCommand => _cancelCommand;

        private readonly RelayCommand<UserApplication> _okCommand;
        public ICommand OkCommand => _okCommand;

        private UserApplication _selectedApplication;
        public UserApplication SelectedApplication
        {
            get => _selectedApplication;
            set
            {
                _selectedApplication = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanAdd));
            }
        }

        public bool CanAdd => _selectedApplication != null;

        public void FindActiveWindows()
        {
            ActiveWindows.Clear();
            foreach (var app in WindowEnumerator.GetAllWindows())
            {
                ActiveWindows.Add(app);
            }
        }

        public ObservableCollection<UserApplication> ActiveWindows { get; } = new ObservableCollection<UserApplication>();

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
