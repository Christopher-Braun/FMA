using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using FMA.Contracts;

namespace FMA.Core
{
    public class MaterialToTextFieldConverter
    {
        private readonly FontService fontService;

        public MaterialToTextFieldConverter(FontService fontService)
        {
            this.fontService = fontService;
        }

        public IEnumerable<TextField> CreateTextFields(CustomMaterial material)
        {
            return material.MaterialFields.Select(CreateTextField);
        }

        private TextField CreateTextField(MaterialField materialField)
        {
            var origin = new Point(materialField.LeftMargin, materialField.TopMargin);

            var fontStyle = materialField.Italic ? FontStyles.Italic : FontStyles.Normal;
            var fontWeight = materialField.Bold ? FontWeights.Bold : FontWeights.Normal;
            var fontFamily = fontService.GetFontFamily(materialField.FontName);

            var typeface = new Typeface(fontFamily, fontStyle, fontWeight, FontStretches.Normal);
            var value =  materialField.Uppper ? materialField.DefaultValue.ToUpper() : materialField.DefaultValue;

            var formattedText = new FormattedText(value, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface, materialField.FontSize, Brushes.Black);

            return new TextField(formattedText, origin);
        }
    }

}
