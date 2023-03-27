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
using WPF_Projects.Models;

namespace WPF_Projects.View.UserControls
{
    /// <summary>
    /// Interaction logic for StateHandlerMaker.xaml
    /// </summary>
    public partial class StateHandlerMaker : UserControl
    {
        private bool? copying;
        public StateHandlerMaker()
        {
            InitializeComponent();
            copying = false;
        }

        public void CreateProfile(object sender, RoutedEventArgs e)
        {
            ProfileMaker.IsEnabled = false;
            try
            {
                var profile = new Profile(ProfileLocation.Text, Src.Text, Dest.Text);

                // Get Current Settings
                var settings = Helpers.LocalStorage.GetSettings();

                if (settings == null)
                {
                    settings = new Settings();
                }

                settings.Profiles.Add(profile);
                Helpers.LocalStorage.SaveSettings(settings);
                Status.Text = "Profile Successfully created";
            }
            catch (Exception ex)
            {
                Status.Text = $"Error: {ex.Message}";
            }
            finally
            {
                ProfileMaker.IsEnabled = true;
            }
        }

        public static async Task CopyFileAsync(string srcFile, string destFile)
        {

            // Open the source file for reading
            using (var sourceStream = new FileStream(srcFile, FileMode.Open, FileAccess.Read))
            {
                // Open the destination file for writing
                using (var destinationStream = new FileStream(destFile, FileMode.Create, FileAccess.Write))
                {
                    // Copy the file asynchronously
                    await sourceStream.CopyToAsync(destinationStream);
                }
            }

            // Check if the file was successfully copied
            if (File.Exists(destFile))
            {
                // File was successfully copied
            }
            else
            {
                // File was not copied
            }
        }

        public static async Task CopyFile(string srcFile, string destFile)
        {
            await CopyFileAsync(srcFile, destFile);
        }

        private async void Copy(object sender, RoutedEventArgs e)
        {
            //if (copying != true)
            //{
            //    copying = true;
            //    CopyStatus.Text = "Copying...";
            //    try
            //    {
            //        btnCopy.IsEnabled = false;
            //        await CopyFile(Src.Text, Dest.Text);
            //        CopyStatus.Text = "Complete";
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //        CopyStatus.Text = $"An error occured: {ex.Message}";
            //    }
            //    finally
            //    {
            //        copying = false;
            //        btnCopy.IsEnabled = true;
            //    }
            //}
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
                Src.Text = openFileDialog.FileName;
                // Do something with the file path
            }
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
                Dest.Text = openFileDialog.FileName;
                // Do something with the file path
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Copy(sender, e);
        }
    }
}
