﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZFileStateHandler.ViewModels
{
    public class DirectoryTrackerViewModel : INotifyPropertyChanged
    {
        private bool _doesExist;
        private string directory;
        private string file;

        public bool IsFile() => (file != "");
        public bool IsDirectory() => (file == "");
        public string GetDirectory() => directory;
        public string GetPath() => (IsFile() ? $"{directory}\\{file}" : directory);

        public DirectoryTrackerViewModel(string dir, string file = "")
        {
            this.directory = dir;
            this.file = file;
            _doesExist = Directory.Exists(directory);
        }

        /// <summary>
        /// Default copied or moved file naming convention using the current data and time
        /// </summary>
        /// <param name="src"></param>
        /// <param name="ext"></param>
        /// <returns></returns>
        private static string DefaultNamingConvention(string? ext, string dest)
        {
            string DateStamp = DateTime.Now.ToString("yyyy-MM-dd");
            string newDir = $"{dest}\\{DateStamp}";
            if (!Directory.Exists(newDir))
            {
                Directory.CreateDirectory(newDir);
            }

            string TimeStamp = DateTime.Now.ToString("HH+mm+ss");
            string fullPath = $"{newDir}\\{TimeStamp}";

            if(ext == "")
                return fullPath;
            return $"${fullPath}\\{ext}";
        }

        /// <summary>
        /// Adds Resources to this Directory
        /// This Method assumes this resource is a directory and will not run, if it is not.
        /// </summary>
        public void Add(string src, bool copy, bool overwrite, Func<string, string, string>? namingConvetion = null)
        {
            if(IsFile())
            {
                Console.WriteLine($"Add Error: {GetPath()} is not a directory.");
                return;
            }

            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

            var dest = GetPath();
            if (namingConvetion == null) namingConvetion = DefaultNamingConvention;
            dest = namingConvetion(Path.GetExtension(src), dest);

            if (copy)
            {
                File.Copy(src, directory, overwrite);
            }
            else
            {
                File.Move(src, directory, overwrite);
            }
        }

        public void CopyTo(string dest, bool overwrite)
        {
            if (IsFile())
            {
                File.Copy(GetPath(), dest, overwrite);
            }
            else
            {
                CopyDirectory(GetPath(), dest, overwrite);
            }
        }

        public void MoveTo(string dest, bool overwrite)
        {
            if (Directory.Exists(dest))
                Directory.Delete(dest, overwrite);

            Directory.Move(GetPath(), dest);

            DoesExist = false;
        }

        public void Delete()
        {
            if (IsFile())
            {
                if (File.Exists(GetPath()))
                    File.Delete(GetPath());
            }
            else
            {
                if (Directory.Exists(GetPath()))
                    Directory.Delete(GetPath(), true);
            }
            DoesExist = false;
        }

        private void CopyDirectory(string src, string dest, bool overwrite)
        {
            // Create Directory
            if (!Directory.Exists(dest))
            {
                Directory.CreateDirectory(dest);
            }

            // Copy Files
            foreach (string file in Directory.GetFiles(src))
            {
                string destFile = Path.Combine(dest, Path.GetFileName(file));
                File.Copy(file, destFile, overwrite);
            }

            // Copy Sub Directoryies
            foreach (string folder in Directory.GetDirectories(src))
            {
                string destDir = Path.Combine(dest, Path.GetFileName(folder));
                CopyDirectory(folder, destDir, overwrite);
            }
        }

        public string? GetMostRecentFile()
        {
            if (IsFile())
            {
                Console.WriteLine($"GetMostRecentFile Error: {GetPath()} is not a directory.");
                return null;
            }

            DirectoryInfo directoryInfo = new DirectoryInfo(GetPath());
            if (directoryInfo.GetFileSystemInfos("*.*").Length == 0) return null;
            FileSystemInfo fileSystemInfo = directoryInfo.GetFileSystemInfos("*.*")[0];

            return fileSystemInfo.Name;
        }

        public bool DoesExist
        {
            get { return _doesExist; }
            set
            {
                if (_doesExist != value)
                {
                    _doesExist = value;
                    OnPropertyChanged(nameof(DoesExist));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}