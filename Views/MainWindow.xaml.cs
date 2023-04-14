using EZFileStateHandler.Helpers;
using System.Windows;
using System.Windows.Controls;
namespace EZFileStateHandler.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainTabs.SelectionChanged += TabControl_SelectionChanged;
        }

        private void StateHandlerMaker_Loaded(object sender, RoutedEventArgs e)
        {
            // Get the screen where you want to display the window
            ScreenHelper.PositionWindow(this, sender, e);
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;

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
            else if (selectedTab == tabViewStates)
            {
                // Reload the contents of the tab
                States.Load();
            }
        }
    }
}
