// Christopher Braun 2016

using System.Linq;
using FMA.Contracts;

namespace FMA.View.Models
{
    public static class MaterialConversions
    {
        public static MaterialModel ToMaterialModel(this Material material, IFontService fontService)
        {
            var defaultFontFamily = new FontFamilyWithName(fontService.GetFontFamily(material.DefaultFont));

            return new MaterialModel(material.Id, material.Title, material.Description,
                material.MaterialFields.Select(f => f.ToMaterialFieldModel(fontService)), material.FlyerFrontSide,
                material.FlyerBackside, defaultFontFamily);
        }

        public static Material ToNewMaterial(this MaterialModel material)
        {
            return new Material(-1, material.Title, material.Description,
                material.MaterialFields.Select(f => f.ToMaterialField()), material.FlyerFrontSide,
                material.FlyerBackside, material.DefaultFont.Name);
        }

        public static CustomMaterial ToCustomMaterial(this MaterialModel material)
        {
            var customLogo = ToCustomLogo(material.LogoModel);

            return new CustomMaterial(material.Id, material.Title, material.Description,
                material.MaterialFields.Select(f => f.ToMaterialField()), material.FlyerFrontSide,
                material.FlyerBackside, customLogo);
        }

        private static CustomLogo ToCustomLogo(LogoModel logoModel)
        {
            var customLogo = new CustomLogo(logoModel.Logo, logoModel.LeftMargin, logoModel.TopMargin, logoModel.Width,
                logoModel.Height);
            return customLogo;
        }

        public static MaterialModel Clone(this MaterialModel material)
        {
            if (material == null) return null;
            return new MaterialModel(material.Id, material.Title, material.Description,
                material.MaterialFields.Select(f => f.Clone()), material.FlyerFrontSide, material.FlyerBackside,
                material.DefaultFont);
        }

        public static MaterialFieldModel ToMaterialFieldModel(this MaterialField field, IFontService fontService)
        {
            var fontFamilyWithName = new FontFamilyWithName(fontService.GetFontFamily(field.FontName));

            return new MaterialFieldModel(field.FieldName, field.Value, fontFamilyWithName, field.FontSize, field.Bold,
                field.Italic, field.Uppper, field.MaxLength, field.MaxRows, field.LeftMargin, field.TopMargin,
                field.TextColor);
        }

        public static MaterialField ToMaterialField(this MaterialFieldModel model)
        {
            var fontName = model.FontFamilyWithName.Name;
            return new MaterialField(model.FieldName, fontName, model.FontSize, model.Bold,
                model.Italic, model.Uppper, model.MaxLength, model.MaxRows, model.LeftMargin,
                model.TopMargin, model.Value, model.TextColor);
        }

        public static MaterialFieldModel Clone(this MaterialFieldModel field)
        {
            return new MaterialFieldModel(field.FieldName, field.Value, field.FontFamilyWithName, field.FontSize,
                field.Bold, field.Italic, field.Uppper, field.MaxLength, field.MaxRows, field.LeftMargin,
                field.TopMargin, field.TextColor);
        }
    }
}