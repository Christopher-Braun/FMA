// Christopher Braun 2016

using System;
using System.Collections.Generic;
using System.Linq;
using FMA.Contracts;
using FMA.View.Helpers;

namespace FMA.View.Common
{
    public class FontHelper
    {
        public static void UpdateFonts(IEnumerable<Material> materials, Func<string, FontInfo> getFont,
            IFontService fontService)
        {
            var requiredFonts = materials.SelectMany(m => m.MaterialFields).Select(f => f.FontName);

            foreach (var requiredFontName in requiredFonts.Where(f => fontService.IsFontAvailable(f) == false))
            {
                var fontInfo = getFont(requiredFontName);
                if (fontInfo != null)
                {
                    fontService.InstallFont(fontInfo.FileName, fontInfo.Buffer);
                }
            }
        }
    }
}