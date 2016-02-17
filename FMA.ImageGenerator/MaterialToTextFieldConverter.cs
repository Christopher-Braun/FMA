using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using FMA.Contracts;

namespace FMA.Core
{
    public class MaterialToTextFieldConverter
    {
        private readonly FontService fontService;
        private readonly BrushConverter brushConverter;

        public MaterialToTextFieldConverter(FontService fontService)
        {
            this.fontService = fontService;
            brushConverter = new BrushConverter();
        }

        public TextField CreateTextField(MaterialField materialField)
        {
       
            var fontStyle = materialField.Italic ? FontStyles.Italic : FontStyles.Normal;
            var fontWeight = materialField.Bold ? FontWeights.Bold : FontWeights.Normal;
            var fontFamily = fontService.GetFontFamily(materialField.FontName);

            var typeface = new Typeface(fontFamily, fontStyle, fontWeight, FontStretches.Normal);
            var value = materialField.Uppper ? materialField.Value.ToUpper() : materialField.Value;

            var textBrush = Brushes.Black;
            try
            {
                textBrush = (SolidColorBrush)brushConverter.ConvertFromString(materialField.TextColor);
            }
            catch (NotSupportedException) { }
            catch (InvalidCastException) { }


            var formattedText = new FormattedText(value, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight, typeface, materialField.FontSize, textBrush);

            var origin = new Point(materialField.LeftMargin, materialField.TopMargin);

            return new TextField(formattedText, origin);
        }
    }

}
