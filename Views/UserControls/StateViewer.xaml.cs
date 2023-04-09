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
        private string transferFileDir = "";
        private string stateQuickDir = "";
        private string fastStateName = "FastState";
        private string statePreviousDir = "";
        private string stateBackupDir = "";

        private string sourceDir = "";
        private string sourceFile = "";

        private bool IsDir(string path) => (File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory;

        private string GetPath(string dir, string file) => (file != "" ? $"{file}/{sourceFile}" : dir);
        private string GetSource() => GetPath(sourceDir, sourceFile);
        private string GetPrevious() => GetPath(statePreviousDir, sourceFile);
        private string GetSpace(string file) => GetPath(transferFileDir, file);

        public StateViewer()
        {
            InitializeComponent();

            stateDir = @"C:\Users\Koy (4-14-2019)\Downloads\ER States";
            transferFileDir = stateDir + @"\Space";
            stateQuickDir = stateDir + @"\Quick";
            statePreviousDir = stateDir + @"\Previous";
            stateBackupDir = stateDir + @"\Backups";

            sourceDir = @"C:\Users\Koy (4-14-2019)\Downloads\Test Dir";
            // sourceFile = "A.txt";

            InitializeFileStructure();

            LoadQuickList();
            LoadPreviousList();
            LoadBackupList();
        }

        private void InitializeFileStructure()
        {
            Directory.CreateDirectory(stateDir);
            Directory.CreateDirectory(stateQuickDir);
            Directory.CreateDirectory(statePreviousDir);
            Directory.CreateDirectory(stateBackupDir);
            InitializeQuickState();
        }

        private string? GetMostRecentFile(string dir, bool checkSubDir = false)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(dir);
            if (directoryInfo.GetFileSystemInfos("*.*").Length == 0) return null;
            FileSystemInfo fileSystemInfo = directoryInfo.GetFileSystemInfos("*.*")[0];

            return fileSystemInfo.Name;
        }

        private void InitializeQuickState()
        {
            if (DirValidation(stateQuickDir, fastStateName)) return;

            // If does not exist, create one of the most recent quick state
            string? mostRecentFile = GetMostRecentFile(stateQuickDir);

            if (mostRecentFile == null)
            {
                ActionStatus.Content = "No files";
                return;
            }

            this.Copy($"{stateQuickDir}", fastStateName);
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
            Load(QuickList, stateQuickDir);
        }
        private void LoadPreviousList()
        {
            Load(PreviousList, statePreviousDir);
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
            if (file != "" && !File.Exists($"{dir}/{file}"))
            {
                var fileName = file != "" ? $"File {file}" : "Source file";
                ActionStatus.Content = $"{fileName} does not exist.";
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
                // From Source To Space
                if (sourceFile == "")
                {
                    Directory.Move(GetSource(), GetSpace(sourceFile));
                    // From Previous To Source
                    Directory.Move(GetPrevious(), GetSource());
                    // From Space To Previous
                    Directory.Move(GetSpace(sourceFile), GetPrevious());
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

        private void Move(string src, string dest)
        {

        }

        private void Copy(string dir, string resultName = "")
        {
            string fullPath;
            if (resultName == "")
            {
                string DateStamp = DateTime.Now.ToString("yyyy-MM-dd");
                Directory.CreateDirectory($"{dir}/{DateStamp}/");
                string TimeStamp = DateTime.Now.ToString("HH+mm+ss");
                fullPath = $"{dir}/{DateStamp}/{TimeStamp}";
            }
            else
            {
                fullPath = $"{dir}/{resultName}";
            }

            if (sourceFile != "")
                File.Copy($"{sourceDir}/{sourceFile}", fullPath, true);
            else
                CopyDirectory(sourceDir, fullPath);
        }

        private void QuickState()
        {
            ActionStatus.Content = "Restoring...";
            try
            {
                if (!SourceValidation()) return;
                this.Copy(stateQuickDir);
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
            Restore();
        }
    }
}
