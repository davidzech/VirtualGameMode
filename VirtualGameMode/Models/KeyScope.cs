using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGameMode.Models
{
    [Serializable]
    public enum KeyScope : int
    {
        AddedApplications = 0,
        FullScreenApplications = 1,
        Global = 2
    }
}
