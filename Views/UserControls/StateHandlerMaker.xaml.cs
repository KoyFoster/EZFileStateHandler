using EZFileStateHandler.Models;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using Ookii.Dialogs.Wpf;
using System.Linq;

namespace EZFileStateHandler.Views.UserControls
{
    /// <summary>
    /// Interaction logic for StateHandlerMaker.xaml
    /// </summary>
    public partial class StateHandlerMaker : UserControl
    {
        private AppSettings AppSettings = (AppSettings)Application.Current.Resources["AppSettings"];

        public StateHandlerMaker()
        {
            InitializeComponent();
            btnCreateProfile.IsEnabled = false;
        }

        // Enable CreateProfile
        private bool IsEnabledCreateProfile => !(tbSrc.Text.Length == 0 || tbDest.Text.Length == 0 || tbProfileName.Text.Length == 0);

        private void ClearAllFields()
        {
            tbSrc.Clear();
            tbDest.Clear();
            tbProfileName.Clear();
            EnableCreateProfile();
        }
        private void EnableCreateProfile()
        {
            btnCreateProfile.IsEnabled = IsEnabledCreateProfile;
        }

        private string SelectFileOrDirectory(bool selectFile = true)
        {
            if (selectFile)
            {
                // Create a new instance of the OpenFileDialog class
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.ShowReadOnly = true;

                // Set the filter for the file types you want to allow the user to select
                openFileDialog.Filter = "All Files (*.*)|*.*";

                // Display the dialog box to the user
                var result = openFileDialog.ShowDialog();

                if (result == true) return openFileDialog.FileName;
                return "";
            }
            else
            {
                // Create a new instance of the OpenFileDialog class
                VistaFolderBrowserDialog openDirDialog = new VistaFolderBrowserDialog();

                // Display the dialog box to the user
                var result = openDirDialog.ShowDialog();
                if (result == true) return openDirDialog.SelectedPath;
            }
            return "";
        }

        private void btnSrc_SelectFile(object sender, RoutedEventArgs e)
        {
            var result = SelectFileOrDirectory();
            if (result != "")
            {
                tbSrc.Text = result;
                EnableCreateProfile();
            }
        }

        private void btnSrc_SelectDir(object sender, RoutedEventArgs e)
        {
            var result = SelectFileOrDirectory(false);
            if (result != "")
            {
                tbSrc.Text = result;
                EnableCreateProfile();
            }
        }

        private void btnDst_SelectFile(object sender, RoutedEventArgs e)
        {
            var result = SelectFileOrDirectory(false);
            if (result != "")
            {
                tbDest.Text = result;
                EnableCreateProfile();
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

        private void ValidateProfileName(object sender, TextChangedEventArgs e)
        {
            if (AppSettings.Settings.Profiles.Where(x => x.Name == tbProfileName.Text).Any())
            {
                ProfileNameErrorMessage.Content = "This profile name is already used.";
                btnCreateProfile.IsEnabled = false;
            }
            else
            {
                ProfileNameErrorMessage.Content = "";
                EnableCreateProfile();
            }

        }

        private void tbProfileLocation_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableCreateProfile();
        }

        public void CreateProfile(object sender, RoutedEventArgs e)
        {
            ProfileMaker.IsEnabled = false;
            try
            {
                var profile = new Profile(tbProfileName.Text, tbSrc.Text, tbDest.Text);

                AppSettings appSettings = (AppSettings)System.Windows.Application.Current.Resources["AppSettings"];
                appSettings.Settings.Profiles.Add(profile);
                appSettings.SaveSettings();
                Status.Text = "Profile Successfully created";
                ClearAllFields();

                // Update other tabs

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
    }
}
