using EZFileStateHandler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZFileStateHandler
{
    public class AppSettings
    {
        public static Settings Settings { get; private set; }

        static AppSettings()
        {
            // Load the settings file on application startup
            Settings = LoadSettings();
        }

        private static Settings LoadSettings()
        {
            var settings = Helpers.LocalStorage.GetSettings();
            if(settings == null) return new Settings();
            return settings;
        }

        public static void SaveSettings()
        {
            Helpers.LocalStorage.SaveSettings(Settings);
        }
    }
}
