using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FMA.Contracts;

namespace FMA.Core
{
    public static class MaterialExtensions
    {
        public static BitmapSource GetFlyerBackground(this CustomMaterial material)
        {
            if (material == null)
            {
                return null;
            }

            var location = Assembly.GetExecutingAssembly().Location;
            var dir = Path.GetDirectoryName(location);
            var flyerFileName = String.Format("Flyer_{0}.jpg", material.Id.ToString("00"));
            var imagePath = Path.Combine(dir, flyerFileName);
            if (File.Exists(imagePath) == false)
            {
                return null;
            }

            return new BitmapImage(new Uri(imagePath));
        }

        public static IEnumerable<TextField> GetTextFields(this CustomMaterial material)
        {
            return material.MaterialFields.Select(GetTextField);
        }

        private static TextField GetTextField(MaterialField materialField)
        {
            //TODO Offset weg
            var origin = new Point(materialField.LeftMargin, materialField.TopMargin + 300);

            var fontStyle = materialField.Italic ? FontStyles.Italic : FontStyles.Normal;
            var fontWeight = materialField.Bold ? FontWeights.Bold : FontWeights.Normal;
            var fontFamily = new FontFamily(materialField.FontName);

            var typeface = new Typeface(fontFamily, fontStyle, fontWeight, FontStretches.Normal);
            var formattedText = new FormattedText(materialField.DefaultValue, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface, materialField.FontSize, Brushes.Black);


            return new TextField(formattedText,origin);
        }
    }

}
