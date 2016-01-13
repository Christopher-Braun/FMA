using System.Diagnostics;
using System.Windows.Media;

namespace FMA.Contracts
{
    /// <summary>
    /// Wird verwendet wegen Problemen beim SourceName von Schriftarten die nicht installiert sind. Dort wird immer der FullPath mit # etc. angezeigt. 
    /// Deshalb hier mit Property Name zum Binden
    /// </summary>
    [DebuggerDisplay("{Name}")]
    public class FontFamilyWithName
    {
        public FontFamilyWithName(FontFamily fontFamily)
        {
            FontFamily = fontFamily;
        }

        public FontFamily FontFamily { get; private set; }

        public string Name
        {
            get { return FontService.GetFamilyName(FontFamily); }
        }

        protected bool Equals(FontFamilyWithName other)
        {
            return Equals(FontFamily, other.FontFamily);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((FontFamilyWithName)obj);
        }

        public override int GetHashCode()
        {
            return (FontFamily != null ? FontFamily.GetHashCode() : 0);
        }
    }
}
