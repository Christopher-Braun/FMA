namespace FMA.View.Helpers
{
    public class FontInfo
    {
        public FontInfo(string fileName, byte[] buffer)
        {
            FileName = fileName;
            Buffer = buffer;
        }

        public string FileName { get; set; }
        public byte[] Buffer { get; set; }
    }
}