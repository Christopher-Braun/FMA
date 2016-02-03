// Christopher Braun 2016

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using FMA.View.Models;

namespace FMA.View.Helpers
{
    public class TextPropertiesCaptionVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var selectedMaterialChilds = value as IEnumerable<MaterialChildModel>;

            return selectedMaterialChilds != null && selectedMaterialChilds.OfType<MaterialFieldModel>().Any()
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}