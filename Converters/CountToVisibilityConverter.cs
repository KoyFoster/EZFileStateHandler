using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace EZFileStateHandler.Converters
{
    public class CountToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue = (int)value > 0;
            return !boolValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility visibilityValue = (Visibility)value;
            return visibilityValue == Visibility.Visible;
        }
    }
}