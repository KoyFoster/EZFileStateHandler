using EZFileStateHandler.Models;
using System.Collections.Generic;

namespace EZFileStateHandler.Interfaces
{
    public interface ISettings
    {
        public List<Profile> Profiles { get; set; }
    }
}
