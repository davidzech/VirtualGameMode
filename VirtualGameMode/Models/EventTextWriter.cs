using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGameMode.Models
{
    public class LineWrittenEventArgs : EventArgs
    {
        public LineWrittenEventArgs(string line)
        {
            Line = line;
        }

        public string Line { get; }
    }

    public class EventTextWriter : TextWriter
    {
        public override Encoding Encoding => System.Text.Encoding.UTF8;

        private readonly StringBuilder _builder = new StringBuilder(256);
        public override void Write(char value)
        {
            _builder.Append(value);
            // check if we end with New Line
            var newLineLength = Environment.NewLine.Length;
            if (_builder.Length < newLineLength)
            {
                return;
            }

            // newLineLength or more characters exist, check the last #newLineLength characters and see if they are equal.
            bool equal = true;
            for (int i = 0, j = _builder.Length - newLineLength ; i < newLineLength; i++, j++)
            {
                if (Environment.NewLine[i] != _builder[j])
                {
                    equal = false;
                    break;
                }
            }

            if (equal)
            {
                // new line detected, invoke eventhandler with string
                var line = _builder.ToString(0, _builder.Length - newLineLength);
                LineWritten?.Invoke(this, new LineWrittenEventArgs(line));
                _builder.Clear();
            }
        }

        public delegate void LineWrittenEventHandler(object sender, LineWrittenEventArgs e);

        public event LineWrittenEventHandler LineWritten;
    }
}
