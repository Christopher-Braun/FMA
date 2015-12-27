using System.Globalization;
using System.Linq;
using System.Windows.Markup;
using System.Windows.Media;
using FMA.Contracts;
using FMA.View.Helpers;

namespace FMA.View.Models
{
    public static class MaterialConversions
    {
        public static MaterialModel ToMaterialModel(this Material material, FontService fontService)
        {
            var defaultFontFamily = fontService.GetFontFamily(material.DefaultFont);

            return new MaterialModel(material.Id, material.Title, material.Description, material.MaterialFields.Select(f => f.ToMaterialFieldModel(fontService)), material.FlyerFrontSide, material.FlyerBackside, defaultFontFamily);
        }

        public static CustomMaterial ToCustomMaterial(this MaterialModel material)
        {
            var customLogo = ToCustomLogo(material.LogoModel);

            return new CustomMaterial(material.Id, material.Title, material.Description, material.MaterialFields.Select(f => f.ToMaterialField()), material.FlyerFrontSide, material.FlyerBackside, customLogo);
        }

        private static CustomLogo ToCustomLogo(LogoModel logoModel)
        {
            var customLogo = new CustomLogo(logoModel.Logo, logoModel.LeftMargin, logoModel.TopMargin, logoModel.Width, logoModel.Height);
            return customLogo;
        }

        public static MaterialModel Clone(this MaterialModel material)
        {
            return new MaterialModel(material.Id, material.Title, material.Description, material.MaterialFields.Select(f => f.Clone()), material.FlyerFrontSide, material.FlyerBackside, material.DefaultFont);
        }

        public static MaterialFieldModel ToMaterialFieldModel(this MaterialField field, FontService fontService)
        {
            var fontFamily = fontService.GetFontFamily(field.FontName);

            return new MaterialFieldModel(field.FieldName, field.DefaultValue, fontFamily, field.FontSize, field.Bold, field.Italic, field.Uppper, field.MaxLength, field.MaxRows, field.LeftMargin, field.TopMargin);
        }

        public static MaterialField ToMaterialField(this MaterialFieldModel fieldModel)
        {
            var fontName = fieldModel.FontFamilyWithName.Name;
            return new MaterialField(fieldModel.FieldName, fontName, fieldModel.FontSize, fieldModel.Bold,
                fieldModel.Italic, fieldModel.Uppper, fieldModel.MaxLength, fieldModel.MaxRows, fieldModel.LeftMargin,
                fieldModel.TopMargin, fieldModel.Value);
        }

        public static MaterialFieldModel Clone(this MaterialFieldModel field)
        {
            return new MaterialFieldModel(field.FieldName, field.Value, field.FontFamily, field.FontSize, field.Bold, field.Italic, field.Uppper, field.MaxLength, field.MaxRows, field.LeftMargin, field.TopMargin);
        }
    }
}
