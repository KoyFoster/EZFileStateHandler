using EZFileStateHandler.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EZFileStateHandler.Views.Controls
{
    public class ProfileComboBox : ComboBox
    {
        private List<Profile> GetProfiles() => ((AppSettings)Application.Current.Resources["AppSettings"]).Settings.Profiles;

        public ProfileComboBox()
        {
            this.LoadProfileList();
        }

        public void LoadProfileList()
        {
            if (GetProfiles().Count > 0)
            {
                if (this.ItemsSource == null)
                    this.Items.Clear();
                this.ItemsSource = GetProfiles().Select(x => x.Name);
                this.SelectedIndex = 0;
            }
            else
            {
                this.ItemsSource = null;
                var cbItem = new ComboBox()
                {
                    Text = "No Profiles"
                };
                this.Items.Add(cbItem);
            }
        }
    }
}
