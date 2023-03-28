using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace EZFileStateHandler
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            // Create an instance of the AppSettings class and add it as a resource
            AppSettings appSettings = new AppSettings();
            Resources.Add("AppSettings", appSettings);
        }
    }
}
