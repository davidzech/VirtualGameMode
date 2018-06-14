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

namespace VirtualGameMode.ViewModels
{
    public class AddApplicationViewModel : INotifyPropertyChanged
    {
        public AddApplicationViewModel(Action<AddApplicationViewModel> cancelAction, Action<AddApplicationViewModel> okAction)
        {
            _cancelCommand = new RelayCommand<object>(o => cancelAction(this));
            _okCommand = new RelayCommand<object>(o => okAction(this), o => { return true; });
            FindActiveWindows();
        }

        private readonly RelayCommand<object> _cancelCommand;
        public ICommand CancelCommand => _cancelCommand;

        private readonly RelayCommand<object> _okCommand;
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
            this.ActiveWindows.Clear();
            this.ActiveWindows.Add(new UserApplication() { Name = "Chrome", ExePath = "c:/..."});
        }

        public ObservableCollection<UserApplication> ActiveWindows { get; } = new ObservableCollection<UserApplication>();

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
