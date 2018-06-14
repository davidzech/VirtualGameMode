using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VirtualGameMode.Annotations;
using VirtualGameMode.Models;

namespace VirtualGameMode.ViewModels
{
    public class LogViewModel : INotifyPropertyChanged
    {
        public LogViewModel()
        {
            Logger.Default.StdOutWriter.ValueWritten += (sender, args) => Text = Text + args.Value;
            Logger.Default.StdErrWriter.ValueWritten += (sender, args) => ErrorText = ErrorText + args.Value;
        }

        private string _text;
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                OnPropertyChanged();
            }
        }

        private string _errorText;

        public string ErrorText
        {
            get => _errorText;
            set
            {
                _errorText = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
