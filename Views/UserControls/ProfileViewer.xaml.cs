using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EZFileStateHandler.Views.UserControls
{
    /// <summary>
    /// Interaction logic for ProfileViewer.xaml
    /// </summary>
    public partial class ProfileViewer : UserControl
    {
        public ProfileViewer()
        {
            InitializeComponent();

            // Initialise profile list
            AppSettings appSettings = (AppSettings)Application.Current.Resources["AppSettings"];
            var profiles = appSettings.Settings.Profiles;
            foreach(var profile in profiles)
            {
                var label = new Label();
                label.Content = profile.Name;
                ProfileList.Children.Add(label);
            }
        }
    }
}
