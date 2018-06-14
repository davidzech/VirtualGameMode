using System;
using System.Text;

namespace VirtualGameMode.Models
{
    [Serializable]
    public class UserApplication
    {
        public string Name { get; set; }
        public string ExePath { get; set; }
    }
}
