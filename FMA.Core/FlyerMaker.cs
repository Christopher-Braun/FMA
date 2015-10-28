using FMA.Contracts;

namespace FMA.Core
{
    public class FlyerMaker
    {
        public static void CreateFlyer(CustomMaterial material, string fileName)
        {
            var image = ImageCreator.CreateImagePreview(material.GetFlyerBackground(), material.GetTextFields());

            ImageSaver.SaveDrawingImage(image, fileName);
        }

        public static byte[] CreateFlyer(CustomMaterial material)
        {
            var image = ImageCreator.CreateImagePreview(material.GetFlyerBackground(), material.GetTextFields());

            return ImageSaver.SaveDrawingImage(image).ToArray();
        }
    }
}
