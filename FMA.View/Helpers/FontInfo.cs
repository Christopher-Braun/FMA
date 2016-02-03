// Christopher Braun 2016

namespace FMA.View.Helpers
{
    public class FontInfo
    {
        public FontInfo(string fileName, byte[] buffer)
        {
            FileName = fileName;
            Buffer = buffer;
        }

        public string FileName { get;  }
        public byte[] Buffer { get; }
    }
}