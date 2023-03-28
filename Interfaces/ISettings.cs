﻿using System.Collections.Generic;
using System.ComponentModel;

namespace EZFileStateHandler.Interfaces
{
    public interface IProfile
    {
        public string Name { get; set; }
        public string Src { get; set; }
        public string Dst { get; set; }
    }

    public interface ISettings
    {
        public List<IProfile> Profiles { get; set; }
    }
}
