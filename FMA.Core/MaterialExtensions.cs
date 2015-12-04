using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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
            return GetBitmapImage(material.FlyerFrontSide);
        }

        public static BitmapSource GetFlyerLogo(this CustomMaterial material)
        {
            if (material.CustomLogo.HasLogo == false)
            {
                return null;
            }

            return GetBitmapImage(material.CustomLogo.Logo);
        }

        public static BitmapImage GetBitmapImage(this byte[] bytes)
        {
            var imageSource = new BitmapImage();
            if (bytes == null)
            {
                return imageSource;
            }

            using (var stream = new MemoryStream(bytes))
            {
                stream.Seek(0, SeekOrigin.Begin);
                imageSource.BeginInit();
                imageSource.StreamSource = stream;
                imageSource.CacheOption = BitmapCacheOption.OnLoad;
                imageSource.EndInit();
            }

            return imageSource;
        }

        public static IEnumerable<TextField> GetTextFields(this CustomMaterial material)
        {
            return material.MaterialFields.Select(ToTextField);
        }

        private static TextField ToTextField(MaterialField materialField)
        {
            var origin = new Point(materialField.LeftMargin, materialField.TopMargin);

            var fontStyle = materialField.Italic ? FontStyles.Italic : FontStyles.Normal;
            var fontWeight = materialField.Bold ? FontWeights.Bold : FontWeights.Normal;
            var fontFamily = new FontFamily(materialField.FontName);

            var typeface = new Typeface(fontFamily, fontStyle, fontWeight, FontStretches.Normal);
            var formattedText = new FormattedText(materialField.DefaultValue, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface, materialField.FontSize, Brushes.Black);


            return new TextField(formattedText, origin);
        }
    }

}
