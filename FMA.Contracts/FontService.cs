using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Markup;
using System.Windows.Media;

namespace FMA.Contracts
{
    /// <summary>
    /// Probleme mit FontCache
    /// see http://stackoverflow.com/questions/5488127/wpf-fonts-getfontfamilies-caches-the-list-of-fonts-how-to-clear-the-cache
    /// </summary>
    public class FontService : IFontService
    {
        private List<FontFamily> customFontFamilies;
        private readonly string customFontsDir;

        public FontService(string customFontsDir)
        {
            this.customFontsDir = customFontsDir;

            ScanFontsFolder();
        }

        public FontFamily GetFontFamily(string fontName)
        {
            // Todo nicht ganz schön aber sollte gehen
            var customFont = customFontFamilies.FirstOrDefault(f => f.FamilyNames.Any(x => x.Value.Equals(fontName)));
            return customFont ?? new FontFamily(fontName);
        }

        internal static string GetFamilyName(FontFamily fontFamily)
        {
            if (fontFamily == null)
            {
                return "Arial";
            }

            try
            {
                //TODO Potentiell gefährlich wenn auf dem Server andere Sprache ist.
                var currentLanguage = CultureInfo.CurrentUICulture.IetfLanguageTag;
                var currentXmlLanguage = XmlLanguage.GetLanguage(currentLanguage);

                if (fontFamily.FamilyNames.ContainsKey(currentXmlLanguage))
                {
                    return fontFamily.FamilyNames[currentXmlLanguage];
                }

                var defaultLanguage = CultureInfo.InvariantCulture.IetfLanguageTag;
                var defaultXmlLanguage = XmlLanguage.GetLanguage(defaultLanguage);

                if (fontFamily.FamilyNames.ContainsKey(defaultXmlLanguage))
                {
                    return fontFamily.FamilyNames[defaultXmlLanguage];
                }

                var fontName = fontFamily.FamilyNames.Values.FirstOrDefault();
                if (string.IsNullOrEmpty(fontName))
                {
                    return fontFamily.Source;
                    //   throw new InvalidOperationException("No FontName for Font available");
                }

                return fontName;
            }
            catch (Exception)
            {
                return fontFamily.Source;
            }

        }

        public bool IsFontAvailable(string fontFamily)
        {
            return AllFontFamilies.Any(x => x.Name.Equals(fontFamily));
        }

        private void ScanFontsFolder()
        {
            customFontFamilies = GetNonCachedFontFamilies(customFontsDir).ToList();
            AllFontFamilies = customFontFamilies.Concat(Fonts.SystemFontFamilies).Select(f => new FontFamilyWithName(f)).ToList();
            // Geht nicht wegen Cache -> Fonts.GetFontFamilies(customFontsDir).ToList();
        }


        public List<FontFamilyWithName> AllFontFamilies { get; private set; }

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

        private static IEnumerable<FontFamily> GetNonCachedFontFamilies(string location)
        {
            if (string.IsNullOrEmpty(location))
            {
                throw new ArgumentOutOfRangeException(nameof(location));
            }

            var directoryInfo = new DirectoryInfo(location);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
                // throw new ArgumentOutOfRangeException("location");
            }

            var fontFiles = directoryInfo.GetFiles("*.?tf");
            return fontFiles.SelectMany(f => Fonts.GetFontFamilies(f.FullName));
        }
    }
}
