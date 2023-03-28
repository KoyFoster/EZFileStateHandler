using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EZFileStateHandler.Interfaces;

namespace EZFileStateHandler.Models
{
    public class Profile : IProfile
    {
        public string Name { get; set; }
        public string Src { get; set; }
        public string Dst { get; set; }

        public Profile(string name, string src, string dst)
        {
            Name = name;
            Src = src;
            Dst = dst;
        }
    }

    public class Settings : ISettings, INotifyPropertyChanged
    {
        private List<IProfile> profiles;
        public List<IProfile> Profiles 
        { 
            get => profiles; 
            set
            {
                profiles = value;
                OnPropertyChanged(nameof(Profiles));
            }
        }

        public List<string> ProfileNames => Profiles.Select(p => p.Name).ToList();

        public Settings()
        {
            Profiles = new List<IProfile>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
