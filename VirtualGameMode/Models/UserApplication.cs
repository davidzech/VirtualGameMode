using System;
using System.Text;

namespace VirtualGameMode.Models
{
    [Serializable]
    public class UserApplication
    {
        public string Name { get; set; }
        public string ExePath { get; set; }

        public String Serialize()
        {
            var nameb64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(Name));
            var exeb64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(ExePath));
            return nameb64 + ";" + exeb64;
        }

        public static UserApplication Deserialize(string obj)
        {
            var split = obj.Split(';');
            var name = Convert.FromBase64String(split[0]);
            var exe = Convert.FromBase64String(split[1]);            
            return new UserApplication()
            {
                Name = Encoding.UTF8.GetString(name),
                ExePath = Encoding.UTF8.GetString(exe)
            };
        } 
    }
}
