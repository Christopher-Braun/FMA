using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            var customFont = customFontFamilies.FirstOrDefault(f => f.FamilyNames.Any(x => x.Value.Equals(fontName)));
            if (customFont != null)
            {
                return customFont;
            }

            return new FontFamily(fontName);
        }

        public bool IsFontAvailable(string fontFamily)
        {
            return customFontFamilies.Concat(Fonts.SystemFontFamilies).Any(f => f.FamilyNames.Any(x => x.Value.Equals(fontFamily)));
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

            return fontFiles.SelectMany(fontFile => Fonts.GetFontFamilies(fontFile.FullName));
        }
    }
}
