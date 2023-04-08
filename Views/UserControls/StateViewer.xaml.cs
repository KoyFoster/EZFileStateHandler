using System;
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
        private string stateDir = "";
        private string stateQuickDir = "";
        private string stateOriginalDir = "";
        private string stateBackupDir = "";

        private string source = "";

        public StateViewer()
        {
            InitializeComponent();

            stateDir = @"C:\Users\Koy (4-14-2019)\Downloads\ER States";
            stateQuickDir = stateDir + @"\Quick";
            stateOriginalDir = stateDir + @"\Original";
            stateBackupDir = stateDir + @"\Backups";

            source = @"C:\Users\Koy (4-14-2019)\Downloads\A.txt";

            InitalizeFileStructure();

            LoadQuickList();
            LoadOriginalList();
            LoadBackupList();
        }

        private void InitalizeFileStructure()
        {
            Directory.CreateDirectory(stateDir);
            Directory.CreateDirectory(stateQuickDir);
            Directory.CreateDirectory(stateOriginalDir);
            Directory.CreateDirectory(stateBackupDir);
        }

        private string[] GetFilesDirectory(string dir)
        {
            string[] files = Directory.GetFiles(dir);

            // Test moving to sub directory
            // Directory.Move(directoryPath);

            return files.Select(file => Path.GetFileName(file)).ToArray();
        }

        private void LoadQuickList()
        {
            Load(BackupList, stateQuickDir);
        }
        private void LoadOriginalList()
        {
            Load(BackupList, stateOriginalDir);
        }
        private void LoadBackupList()
        {
            Load(BackupList, stateBackupDir);
        }

        public void Load(StackPanel panel, string dir)
        {
            try
            {
                // Initialise profile list
                var files = GetFilesDirectory(dir);

                QuickList.Children.Clear();
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

        private void Restore()
        {
            ActionStatus.Content = "Restoring...";
            try
            {
                // Confirm Source Exists
                if (!File.Exists(source))
                {
                    ActionStatus.Content = "Source files do not exist.";
                    return;
                }
                // Confirm Restore File Exists
            }
            catch (Exception ex)
            {
                ActionStatus.Content = ex.Message;
            }
            ActionStatus.Content = "Restore Completed";
        }

        private void btnQuickState_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btBackup_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnQuickRestore_Click(object sender, RoutedEventArgs e)
        {
            Restore();
        }

        private void btnBackupRestore_Click(object sender, RoutedEventArgs e)
        {
            Restore();
        }

        private void btnRevertRestore_Click(object sender, RoutedEventArgs e)
        {
            Restore();
        }
    }
}
