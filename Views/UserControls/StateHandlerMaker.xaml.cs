using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using EZFileStateHandler.Models;
using EZFileStateHandler.Interfaces;

namespace EZFileStateHandler.Views.UserControls
{
    /// <summary>
    /// Interaction logic for StateHandlerMaker.xaml
    /// </summary>
    public partial class StateHandlerMaker : UserControl
    {

        public StateHandlerMaker()
        {
            InitializeComponent();
            btnCreateProfile.IsEnabled = false;
        }

        // Enable CreateProfile
        private bool IsEnabledCreateProfile => !(tbSrc.Text.Length == 0 || tbDest.Text.Length == 0 || tbProfileName.Text.Length == 0 || tbProfileLocation.Text.Length == 0);

        private void ClearAllFields()
        {
            tbSrc.Clear();
            tbDest.Clear();
            tbProfileName.Clear();
            tbProfileLocation.Clear();
            EnableCreateProfile();
        }
        private void EnableCreateProfile()
        {
            btnCreateProfile.IsEnabled = IsEnabledCreateProfile;
        }

        private void btnSrc_SelectFile(object sender, RoutedEventArgs e)
        {
            // Create a new instance of the OpenFileDialog class
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Set the filter for the file types you want to allow the user to select
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

            // Display the dialog box to the user
            bool? result = openFileDialog.ShowDialog();

            // If the user selects a file, get the file path
            if (result == true)
            {
                tbSrc.Text = openFileDialog.FileName;
                // Do something with the file path
            }

            EnableCreateProfile();
        }

        private void btnDst_SelectFile(object sender, RoutedEventArgs e)
        {
            // Create a new instance of the OpenFileDialog class
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Set the filter for the file types you want to allow the user to select
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

            // Display the dialog box to the user
            bool? result = openFileDialog.ShowDialog();

            // If the user selects a file, get the file path
            if (result == true)
            {
                tbDest.Text = openFileDialog.FileName;
                // Do something with the file path
            }

        }

        public void CreateProfile(object sender, RoutedEventArgs e)
        {
            ProfileMaker.IsEnabled = false;
            try
            {
                var profile = new Profile(tbProfileLocation.Text, tbSrc.Text, tbDest.Text);

                AppSettings appSettings = (AppSettings)Application.Current.Resources["AppSettings"];
                appSettings.Settings.Profiles.Add(profile);
                appSettings.SaveSettings();
                Status.Text = "Profile Successfully created";
                // Clear Fields

                ClearAllFields();
            }
            catch (Exception ex)
            {
                Status.Text = $"Error: {ex.Message}";
                btnCreateProfile.IsEnabled = false;
            }
            finally
            {
                ProfileMaker.IsEnabled = true;
            }
        }

        private void tbSrc_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableCreateProfile();
        }

        private void tbDest_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableCreateProfile();
        }

        private void tbProfileName_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableCreateProfile();
        }

        private void tbProfileLocation_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableCreateProfile();
        }
    }
}
