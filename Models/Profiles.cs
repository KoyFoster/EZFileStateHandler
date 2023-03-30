using EZFileStateHandler.Interfaces;

namespace EZFileStateHandler.Models
{
    public class Profile : IProfile
    {
        public string Name { get; set; }
        public string Src { get; set; }
        public string Dst { get; set; }

        public Profile(string name, string src, string dst)
        {
            Name = name;
            Src = src;
            Dst = dst;
        }
    }
}
