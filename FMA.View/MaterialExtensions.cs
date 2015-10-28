using System.Linq;
using FMA.Contracts;

namespace FMA.View
{
   public static class MaterialExtensions
    {
       public static MaterialModel ToMaterialModel(this Material material)
       {
           return new MaterialModel(material.Id, material.Title, material.Description, material.MaterialFields.Select(f => f.ToMaterialFieldModel()));
       }

       public static CustomMaterial ToCustomMaterial(this MaterialModel material)
       {
           return new CustomMaterial(material.Id, material.Title, material.Description, material.MaterialFields.Select(f => f.ToMaterialField()));
       }   
       
       public static MaterialModel Clone(this MaterialModel material)
       {
           return new MaterialModel(material.Id, material.Title, material.Description, material.MaterialFields.Select(f => f.Clone()));
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
