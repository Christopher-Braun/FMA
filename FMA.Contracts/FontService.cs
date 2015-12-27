using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace FMA.Contracts
{
    /// <summary>
    /// Probleme mit FontCache
    /// see http://stackoverflow.com/questions/5488127/wpf-fonts-getfontfamilies-caches-the-list-of-fonts-how-to-clear-the-cache
    /// </summary>
    public class FontService
    {
        private List<FontFamily> customFontFamilies;
        private readonly string customFontsDir;

        public FontService(string customFontsDir)
        {
            this.customFontsDir = customFontsDir;

            ScanFontsFolder();
        }

        public void ScanFontsFolder()
        {
            customFontFamilies = GetNonCachedFontFamilies(customFontsDir).ToList();
            // Geht nicht wegen Cache -> Fonts.GetFontFamilies(customFontsDir).ToList();
        }

        public FontFamily GetFontFamily(string fontName)
        {
            // Todo nicht ganz schön aber sollte gehen
            var customFont = customFontFamilies.FirstOrDefault(f => f.FamilyNames.Any(x => x.Value.Equals(fontName)));
            if (customFont != null)
            {
                return customFont;
            }

            return new FontFamily(fontName);
        }

        internal static string GetFamilyName(FontFamily fontFamily)
        {
            //TODO Potentiell gefährlich wenn auf dem Server andere Sprache ist.
            var currentLanguage = CultureInfo.CurrentUICulture.IetfLanguageTag;
            var currentXmlLanguage = XmlLanguage.GetLanguage(currentLanguage);
            var fontName = fontFamily.FamilyNames[currentXmlLanguage];
            return fontName;

        }

        public bool IsFontAvailable(string fontFamily)
        {
            return AllFontFamilies.Any(x => x.Name.Equals(fontFamily));
        }

        public IEnumerable<FontFamilyWithName> AllFontFamilies
        {
            get
            {
                return customFontFamilies.Concat(Fonts.SystemFontFamilies).Select(f => new FontFamilyWithName(f));
            }
        }

        public void InstallFont(string fileName, byte[] font)
        {
            var fullName = Path.Combine(customFontsDir, fileName);

            using (var memoryStream = new MemoryStream(font))
            using (var stream = new FileStream(fullName, FileMode.OpenOrCreate))
            {
                memoryStream.WriteTo(stream);
            }

            ScanFontsFolder();
        }

        public static IEnumerable<FontFamily> GetNonCachedFontFamilies(string location)
        {
            if (string.IsNullOrEmpty("location"))
            {
                throw new ArgumentOutOfRangeException("location");
            }

            var directoryInfo = new DirectoryInfo(location);
            if (!directoryInfo.Exists)
            {
                throw new ArgumentOutOfRangeException("location");
            }

            var fontFiles = directoryInfo.GetFiles("*.?tf");
            return fontFiles.SelectMany(f => Fonts.GetFontFamilies(f.FullName));

            //return fontFiles.Select(fontFile =>
            //{
            //    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fontFile.FullName);
            //    var path = String.Format("file:///{0}#{1}", FileInfo.FullName, fileNameWithoutExtension);
            //    return new FontFamily(path,fileNameWithoutExtension);
            //});
        }
    }
}
