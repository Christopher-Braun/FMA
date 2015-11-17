using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using FMA.Contracts;

namespace FMA.TestData
{
    public class DummyData
    {
        public static List<Material> GetDummyMaterials()
        {
            var materials = new List<Material>();

            var material1Fields = new List<MaterialField>
            {
                new MaterialField("Referent", "Arial", 10, false, false, false, 50, 1, 7, 371, "Paulus von Tarsus"),
                new MaterialField("Titel", "Arial", 21, true, false, true, 50, 3, 7, 320, "Titel 1"),
                new MaterialField("Untertitel", "Times New Roman", 12, true, false, true, 80, 3, 7, 315, "Untertitel 1")
            };

            var material1 = new Material(1, "Gefährlicher Glaube", "ZumThema Gefährlicher Glaube (Islamische Welt)",
                material1Fields, GetFrontSide(1), GetFrontSide(1));

            var material2Fields = new List<MaterialField>
            {
                new MaterialField("Referent", "Arial", 10, false, false, false, 50, 1, 7, 371, "Petrus"),
                new MaterialField("Titel", "Arial", 21, true, false, true, 50, 3, 7, 320, "Nordkorea - Gestern und Heute"),
                new MaterialField("Untertitel", "Times New Roman", 12, true, false, true, 80, 3, 7, 315,
                    "Untertitel Nordkorea"),
                new MaterialField("Ort", "Times New Roman", 16, false, true, false, 80, 3, 7, 383, "Hamburg")
            };

            var material2 = new Material(2, "Nordkorea", "Zum Thema Nordkorea…", material2Fields, GetFrontSide(2), GetFrontSide(2));

            materials.Add(material1);
            materials.Add(material2);

            return materials;
        }

        public static CustomMaterial GetCustomMaterial()
        {
            var material2Fields = new List<MaterialField>
            {
                new MaterialField("Referent", "Arial", 10, false, false, false, 50, 1, 7, 71, "Petrus"),
                new MaterialField("Titel", "Arial", 21, true, false, true, 50, 3, 7, 20, "Nordkorea - SilentMode"),
                new MaterialField("Untertitel", "Times New Roman", 12, true, false, true, 80, 3, 7, 15,
                    "Untertitel Nordkorea"),
                new MaterialField("Ort", "Times New Roman", 16, false, true, false, 80, 3, 7, 83, "Hamburg")
            };
            return new CustomMaterial(2, "Nordkorea", "Zum Thema Nordkorea…", material2Fields ,GetFrontSide(2), GetFrontSide(2), new CustomLogo());
        }

        public static Byte[] GetFrontSide(int id)
        {
            var location = Assembly.GetEntryAssembly().Location;
            var dir = Path.GetDirectoryName(location);
            var flyerFileName = String.Format("Flyer_{0}.jpg", id.ToString("00"));
            var imagePath = Path.Combine(dir, flyerFileName);
            if (File.Exists(imagePath) == false)
            {
                return null;
            }

            byte[] data;

            using (var fileStream = new FileStream(imagePath, FileMode.Open))
            {
                using (var ms = new MemoryStream())
                {
                    fileStream.CopyTo(ms);
                    data = ms.ToArray();
                }
            }

            return data;
        }
    }
}