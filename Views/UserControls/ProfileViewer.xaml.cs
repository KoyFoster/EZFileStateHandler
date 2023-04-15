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
        private bool HasChanged = false;
        private void SetChanged(bool changed) { HasChanged = changed; btnUndo.IsEnabled = HasChanged; }

        public ProfileViewer()
        {
            InitializeComponent();
            LoadProfile();
        }

        public void Reload()
        {
        }

        private void LoadProfile()
        {
            var profile = ProfileList.GetProfile();
            if(profile != null)
            {
                tbDest.Text = profile.Dst;
                tbSrc.Text = profile.Src;
                tbProfileName.Text = profile.Name;
            }
        }

        private void tbSrc_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetChanged(true);
        }

        private void tbDest_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetChanged(true);

        }

        private void tbProfileName_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetChanged(true);

        }

        private void btnSelDirSrc_Click(object sender, RoutedEventArgs e)
        {
            SetChanged(true);

        }

        private void btnSelFileSrc_Click(object sender, RoutedEventArgs e)
        {
            SetChanged(true);

        }

        private void btnSelDest_Click(object sender, RoutedEventArgs e)
        {
            SetChanged(true);

        }

        private void btnCreateProfile_Click(object sender, RoutedEventArgs e)
        {
            SetChanged(true);

        }

        private void ProfileList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadProfile();
            SetChanged(false);
        }

        private void btnUndo_Click(object sender, RoutedEventArgs e)
        {
            LoadProfile();
            SetChanged(false);
        }
    }
}
