﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    public class Settings : ISettings
    {
        public List<IProfile> Profiles { get; set; }

        public Settings()
        {
            Profiles = new List<IProfile>();
        }
    }
}
