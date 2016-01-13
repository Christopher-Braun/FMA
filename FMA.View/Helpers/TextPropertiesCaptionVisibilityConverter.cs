using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using FMA.View.Models;

namespace FMA.View.Helpers
{
    public class TextPropertiesCaptionVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is MaterialFieldModel ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}