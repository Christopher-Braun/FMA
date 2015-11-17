using System.Linq;
using FMA.Contracts;
using FMA.Core;

namespace FMA.View
{
    public static class MaterialConversions
    {
        public static MaterialModel ToMaterialModel(this Material material)
        {
            return new MaterialModel(material.Id, material.Title, material.Description, material.MaterialFields.Select(f => f.ToMaterialFieldModel()), material.FlyerFrontSide, material.FlyerBackside);
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
            return new MaterialModel(material.Id, material.Title, material.Description, material.MaterialFields.Select(f => f.Clone()), material.FlyerFrontSide, material.FlyerBackside);
        }

        public static MaterialFieldModel ToMaterialFieldModel(this MaterialField field)
        {
            return new MaterialFieldModel(field.FieldName, field.FontName, field.FontSize, field.Bold, field.Italic, field.Uppper, field.MaxLength, field.MaxRows, field.LeftMargin, field.TopMargin, field.DefaultValue);
        }

        public static MaterialField ToMaterialField(this MaterialFieldModel fieldModel)
        {
            return new MaterialField(fieldModel.FieldName, fieldModel.FontName, fieldModel.FontSize, fieldModel.Bold, fieldModel.Italic, fieldModel.Uppper, fieldModel.MaxLength, fieldModel.MaxRows, fieldModel.LeftMargin, fieldModel.TopMargin, fieldModel.Value);
        }

        public static MaterialFieldModel Clone(this MaterialFieldModel field)
        {
            return new MaterialFieldModel(field.FieldName, field.FontName, field.FontSize, field.Bold, field.Italic, field.Uppper, field.MaxLength, field.MaxRows, field.LeftMargin, field.TopMargin, field.Value);
        }
    }
}
