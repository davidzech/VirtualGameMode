using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGameMode.Models
{
    public class ValueWrittenEventArgs : EventArgs
    {
        public ValueWrittenEventArgs(char value)
        {
            Value = value;
        }

        public char Value { get; }
    }

    public class EventTextWriter : TextWriter
    {
        public override Encoding Encoding => System.Text.Encoding.UTF8;

        public override void Write(char value)
        {
            ValueWritten?.Invoke(this, new ValueWrittenEventArgs(value));
        }

        public delegate void ValueWrittenEventHandler(object sender, ValueWrittenEventArgs e);

        public event ValueWrittenEventHandler ValueWritten;
    }
}
