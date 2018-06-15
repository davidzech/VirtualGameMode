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
        private const int _maxLog = 500;
        public LogViewModel()
        {
            Logger.Default.StdOutWriter.LineWritten += (sender, args) =>
            {
                var time = DateTime.Now.ToString("[HH:mm] ");
                _textList.AddLast(time + args.Line);
                if (_textList.Count > _maxLog)
                {
                    _textList.RemoveFirst();
                }
                Text = _textList.Aggregate((s, s1) => s + Environment.NewLine + s1);
            };
            Logger.Default.StdErrWriter.LineWritten += (sender, args) =>
            {
                _errorTextList.AddLast(args.Line);
                if (_errorTextList.Count > _maxLog)
                {
                    _errorTextList.RemoveFirst();
                }

                ErrorText = _errorTextList.Aggregate((s, s1) => s + Environment.NewLine + s1);
            };
        }


        private readonly LinkedList<string> _textList = new LinkedList<string>();
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

        private readonly LinkedList<string> _errorTextList = new LinkedList<string>();
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
