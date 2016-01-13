using System.Collections.Generic;
using System.Windows.Media;

namespace FMA.Contracts
{
    public interface IFontService
    {
        FontFamily GetFontFamily(string fontName);
        bool IsFontAvailable(string fontFamily);
        List<FontFamilyWithName> AllFontFamilies { get; }
        void InstallFont(string fileName, byte[] font);
    }
}