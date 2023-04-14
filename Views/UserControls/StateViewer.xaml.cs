using EZFileStateHandler.Models;
using EZFileStateHandler.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace EZFileStateHandler.Views.UserControls
{
    /// <summary>
    /// Interaction logic for StateViewer.xaml
    /// </summary>
    public partial class StateViewer : UserControl
    {
        // Directories
        // Source
        DirectoryTrackerViewModel? DtvmSource { get; set; }
        DirectoryTrackerViewModel? DtvmPrevious { get; set; }
        DirectoryTrackerViewModel? DtvmBackups { get; set; }
        DirectoryTrackerViewModel? DtvmStates { get; set; }

        private string stateDir = "";
        private string transferFileDir = "";
        private string statePreviousDir = "";
        private string stateBackupDir = "";

        private string sourceDir = "";
        private string sourceFile = "";

        private string GetPath(string dir, string file) => (file != "" ? $"{file}/{sourceFile}" : dir);
        private string GetSource() => GetPath(sourceDir, sourceFile);
        private string GetPrevious() => GetPath(statePreviousDir, sourceFile);
        private string GetSpace(string file = "") => GetPath(transferFileDir, file);

        private List<Profile> GetProfiles() => ((AppSettings)Application.Current.Resources["AppSettings"]).Settings.Profiles;

        public StateViewer()
        {
            InitializeComponent();
            this.Load();
        }

        public void Load()
        {
            LoadProfileList();

            var selectedProfile = GetSelectedProfile();
            if (selectedProfile == null) return;
            InitPaths(selectedProfile.Src, selectedProfile.Dst);

            InitializeFileStructure();

            LoadQuickList();
            LoadBackupList();
        }

        public void InitPaths(string src, string stateDir)
        {
            // check source for if it is a file or directory
            string? srcDir, srcFile;
            // Very weak check at the moment
            srcDir = Path.GetDirectoryName(src);
            if (File.Exists(src)) srcFile = Path.GetFileName(src);
            else srcFile = "";
            
            if(srcDir == null)
            {
                ActionStatus.Content = $"Invalid path in profile {src}";
                return;
            }

            this.stateDir = stateDir;
            this.sourceDir = srcDir;
            this.sourceFile = srcFile;

            transferFileDir = stateDir + @"\Space";
            statePreviousDir = stateDir + @"\Previous";
            stateBackupDir = stateDir + @"\Backups";

            // Init directories
            DtvmSource = new DirectoryTrackerViewModel(srcDir, srcFile);

            DtvmStates = new DirectoryTrackerViewModel($"{stateDir}\\Quick");
            QuickList.DataContext = DtvmStates;
            QuickListEmpty.DataContext = DtvmBackups;

            DtvmBackups = new DirectoryTrackerViewModel($"{stateDir}\\Backups");
            BackupList.DataContext = DtvmBackups;
            BackupListEmpty.DataContext = DtvmBackups;

            DtvmPrevious = new DirectoryTrackerViewModel($"{stateDir}\\Previous");
            btnRevertRestore.DataContext = DtvmPrevious;
        }

        public void LoadProfileList()
        {
            if (GetProfiles().Count > 0)
            {
                if(ProfileList.ItemsSource == null)
                ProfileList.Items.Clear();
                ProfileList.ItemsSource = GetProfiles().Select(x => x.Name);
                ProfileList.SelectedIndex = 0;
            }
        }

        public Profile? GetSelectedProfile()
        {
            if (GetProfiles().Count == 0) return null;
            return GetProfiles().Where(x => x.Name == ProfileList.SelectedItem.ToString()).FirstOrDefault();
        }

        public void Refresh()
        {
            DtvmPrevious?.Refresh();
            LoadQuickList();
            DtvmStates?.Refresh();
            LoadBackupList();
            DtvmBackups?.Refresh();
        }

        private void InitializeFileStructure()
        {
            if (!Directory.Exists(stateDir))
                Directory.CreateDirectory(stateDir);

            if (!Directory.Exists(transferFileDir))
                Directory.CreateDirectory(transferFileDir);

            if (!Directory.Exists(statePreviousDir))
                btnRevertRestore.IsEnabled = false;

            if (!Directory.Exists(stateBackupDir))
                Directory.CreateDirectory(stateBackupDir);
        }

        private string[] GetFilesDirectory(string dir)
        {
            string[] files = Directory.GetFiles(dir);
            return files.Select(file => Path.GetFileName(file)).ToArray();
        }

        private void LoadQuickList()
        {
            if(DtvmStates != null)
            {
                QuickList.Children.Clear();
                foreach (string file in DtvmStates.GetFiles())
                {
                    var label = new Label
                    {
                        Content = file
                    };
                    QuickList.Children.Add(label);
                }
            }
        }
        private void LoadBackupList()
        {
            if(DtvmBackups != null)
            {
                BackupList.Children.Clear();
                foreach (string file in DtvmBackups.GetFiles())
                {
                    var label = new Label
                    {
                        Content = file
                    };
                    BackupList.Children.Add(label);
                }
            }
        }

        public void Load(StackPanel panel, string dir)
        {
            try
            {
                // Initialise profile list
                var files = GetFilesDirectory(dir);

                panel.Children.Clear();
                foreach (var file in files)
                {
                    var label = new Label();
                    label.Content = file;
                    panel.Children.Add(label);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private bool DirValidation(string dir, string file, string dirname = "")
        {
            var dirName = dirname != "" ? $"{dirname} directory" : "Directory";
            // Confirm sourceDir Exists
            if (!Directory.Exists(dir))
            {
                ActionStatus.Content = $"{dirName} does not exist.";
                return false;
            }
            // Confirm source file exists if one is given
            var filePath = $"{dir}\\{file}";
            if (file != "" && !File.Exists(filePath))
            {
                ActionStatus.Content = $"{filePath} does not exist.";
                return false;
            }
            return true;
        }

        private bool SourceValidation()
        {
            return DirValidation(sourceDir, sourceFile);
        }

        /// <summary>
        /// Restores source files from previous overwrite
        /// </summary>
        private void Restore()
        {
            try
            {
                // From Source To Space, Previous to Source, Space to Previous
                if (sourceFile == "")
                {
                    if (Directory.Exists(GetSpace())) Directory.Delete(GetSpace(), true);
                    Directory.Move(GetSource(), GetSpace());
                    // From Previous To Source
                    Directory.Move(GetPrevious(), GetSource());
                    //// From Space To Previous
                    Directory.Move(GetSpace(), GetPrevious());
                }
                else
                {
                    File.Move(GetSource(), GetSpace(sourceFile));
                    // From Previous To Source
                    File.Move(GetPrevious(), GetSource());
                    // From Space To Previous
                    File.Move(GetSpace(sourceFile), GetPrevious());
                }
            }
            catch (Exception ex)
            {
                ActionStatus.Content = ex.Message;
            }
        }

        private void RestoreFromPrevious()
        {
            try
            {
                // From Source To Space, Previous to Source, Space to Previous
                if (sourceFile == "")
                {
                    if (Directory.Exists(GetSpace())) Directory.Delete(GetSpace(), true);
                    Directory.Move(GetSource(), GetSpace());
                    // From Previous To Source
                    Directory.Move(GetPrevious(), GetSource());
                    //// From Space To Previous
                    Directory.Move(GetSpace(), GetPrevious());
                }
                else
                {
                    File.Move(GetSource(), GetSpace(sourceFile));
                    // From Previous To Source
                    File.Move(GetPrevious(), GetSource());
                    // From Space To Previous
                    File.Move(GetSpace(sourceFile), GetPrevious());
                }
            }
            catch (Exception ex)
            {
                ActionStatus.Content = ex.Message;
            }
        }

        private void CopyDirectory(string src, string dest)
        {
            if (!Directory.Exists(dest))
            {
                Directory.CreateDirectory(dest);
            }

            foreach (string file in Directory.GetFiles(src))
            {
                string destFile = Path.Combine(dest, Path.GetFileName(file));
                File.Copy(file, destFile, true);
            }

            foreach (string folder in Directory.GetDirectories(src))
            {
                string destDir = Path.Combine(dest, Path.GetFileName(folder));
                CopyDirectory(folder, destDir);
            }
        }

        private void QuickState()
        {
            ActionStatus.Content = "Restoring...";
            try
            {
                if (!SourceValidation()) return;
                // this.Copy(stateQuickDir);
                if(DtvmStates != null && DtvmSource != null)
                DtvmStates.Add(DtvmSource.GetPath(), true, true);
            }
            catch (Exception ex)
            {
                ActionStatus.Content = ex.Message;
                return;
            }
            ActionStatus.Content = "Quick Save Completed";
        }

        private void btnQuickState_Click(object sender, RoutedEventArgs e)
        {
            QuickState();
        }

        private void btBackup_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnQuickRestore_Click(object sender, RoutedEventArgs e)
        {
            // This takes the most recent state backup for near instant restoration
            // If enabled, set previous state as next quick restore
            // This assumes that a copy of said state was make for direct movement
            Restore();
        }

        private void btnBackupRestore_Click(object sender, RoutedEventArgs e)
        {
            Restore();
        }

        private void btnRevertRestore_Click(object sender, RoutedEventArgs e)
        {
            RestoreFromPrevious();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void ProfileList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;

            if (ProfileList.SelectedIndex < 0) return;
            var selectedProfile = GetSelectedProfile();
            if (selectedProfile == null) return;
            InitPaths(selectedProfile.Src, selectedProfile.Dst);
        }
    }
}
