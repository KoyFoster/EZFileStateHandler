using System.Windows;
using System.Windows.Controls;

namespace EZFileStateHandler.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StateHandlerMaker_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void TabControl_SelectionChanged(object sender, RoutedEventArgs e)
        {
            // Get the selected tab
            var selectedTab = (TabItem)MainTabs.SelectedItem;

            // Check if the selected tab is the tab that you want to reload
            if (selectedTab == tabCreateProfile)
            {
                // Reload the contents of the tab
            }
            else if (selectedTab == tabViewProfiles)
            {
                // Reload the contents of the tab
                Profiles.Reload();
            }
        }
    }
}
