using EZFileStateHandler.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZFileStateHandler
{
    public class AppSettings
    {
        public Settings Settings { get; set; }

        public AppSettings()
        {
            // Load the settings file on instantiation
            Settings = LoadSettings();
        }

        private Settings LoadSettings()
        {
            var settings = Helpers.LocalStorage.GetSettings();
            if (settings == null) return new Settings();
            return settings;
        }

        public void SaveSettings()
        {
            Helpers.LocalStorage.SaveSettings(Settings);
        }


    }
}
