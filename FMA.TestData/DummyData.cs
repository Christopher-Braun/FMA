using System.Collections.Generic;
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
                new MaterialField("Veranstalter", "Arial", 12, false, false, true, 50, 3, 24, 305, "FeG Hanau"),
                new MaterialField("Titel", "Arial", 30, true, false, true, 50, 2, 24, 350, "Verfolgt, weil sie \ran Jesus Glauben"),
                new MaterialField("Untertitel", "Times New Roman", 16, true, false, true, 80, 3, 24, 430, "Einladung zum Bibelabend Vitamin B3 "),
                new MaterialField("Datum", "Times New Roman", 22, true, false, true, 80, 3, 24, 500, "FR 11.07.2014 / 20:00 "),
                new MaterialField("Untertitel 2", "Times New Roman", 16, false, true, false, 80, 3, 24, 530, "Vortrag mit aktuellen Berichten"),
                new MaterialField("Referent", "Arial", 12, false, false, false, 80, 4, 24, 600, "Freie evangelische Gemeinde Hanau\rWeimarer Straße 35\r63454 Hanau-Weststadt "),
            };

            var material1 = new Material(1, "Gefährlicher Glaube", "ZumThema Gefährlicher Glaube (Islamische Welt)",
                material1Fields, Helper.GetFrontSide(1), Helper.GetFrontSide(1));

            var material2Fields = new List<MaterialField>
            {
                new MaterialField("Referent", "Signarita Anne", 120, false, false, false, 50, 1, 40, 600, "Petrus"),
                new MaterialField("Titel", "Arial", 21, true, false, true, 50, 3, 7, 320, "Nordkorea - Gestern und Heute"),
                new MaterialField("Untertitel", "Times New Roman", 12, true, false, true, 80, 3, 27, 350,"Untertitel Nordkorea"),
                new MaterialField("Ort", "Bakery", 46, false, true, false, 80, 3, 60, 520, "Hamburg")
            };

            var material2 = new Material(2, "Nordkorea", "Zum Thema Nordkorea…", material2Fields, Helper.GetFrontSide(2), Helper.GetFrontSide(2),"Bakery");

            materials.Add(material1);
            materials.Add(material2);

            return materials;
        }

        public static CustomMaterial GetCustomMaterial()
        {
            var material2Fields = new List<MaterialField>
            {
                new MaterialField("Referent", "Signarita Anne", 16, false, false, false, 50, 1, 7, 71, "Petrus"),
                new MaterialField("Titel", "Arial", 21, true, false, true, 50, 3, 7, 20, "Nordkorea - SilentMode"),
                new MaterialField("Untertitel", "Times New Roman", 12, true, false, true, 80, 3, 7, 15,
                    "Untertitel Nordkorea"),
                new MaterialField("Ort", "Times New Roman", 16, false, true, false, 80, 3, 7, 83, "Hamburg")
            };
            return new CustomMaterial(2, "Nordkorea", "Zum Thema Nordkorea…", material2Fields , Helper.GetFrontSide(2), Helper.GetFrontSide(2), new CustomLogo());
        }
    }
}