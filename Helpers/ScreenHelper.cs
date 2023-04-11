using System;
using System.Linq;
using System.Windows;
using System.Windows.Forms;

namespace EZFileStateHandler.Helpers
{
    public class ScreenHelper
    {        
        public static void PositionWindow(Window win, object sender, RoutedEventArgs e)
        {
            // Get the screen where you want to display the window
            Screen screen = Screen.AllScreens.LastOrDefault();

            if (screen != null)
            {
                // Set the window's startup location to the top-right corner of the screen
                win.WindowStartupLocation = WindowStartupLocation.Manual;
                win.Left = screen.Bounds.Right - win.Width;
                win.Top = screen.Bounds.Top;
            }
        }
    }
}
