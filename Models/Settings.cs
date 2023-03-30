using EZFileStateHandler.Interfaces;
using System.Collections.Generic;

namespace EZFileStateHandler.Models
{
    public class Settings : ISettings
    {
        public List<Profile> Profiles { get; set; }

        public Settings()
        {
            Profiles = new List<Profile>();
        }
    }
}
