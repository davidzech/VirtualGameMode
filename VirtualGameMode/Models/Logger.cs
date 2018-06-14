using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGameMode.Models
{
    public class Logger
    {
        private readonly EventTextWriter _stdOutWriter;
        private readonly EventTextWriter _stdErrWriter;
        private Logger()
        {
            _stdOutWriter = new EventTextWriter();
            _stdErrWriter = new EventTextWriter();
            Console.SetOut(_stdOutWriter);
            Console.SetError(_stdErrWriter);
        }

        public EventTextWriter StdOutWriter => _stdOutWriter;
        public EventTextWriter StdErrWriter => _stdErrWriter;

        public static Logger Default { get; } = new Logger();

    }
}
