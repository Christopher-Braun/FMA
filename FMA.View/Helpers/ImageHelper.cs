using System.Drawing;
using System.IO;

namespace FMA.View.Helpers
{
    public class ImageHelper
    {
        public static byte[] ImageToByte(Image img)
        {
            var byteArray = new byte[0];
            using (var stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                stream.Close();

                byteArray = stream.ToArray();
            }
            return byteArray;
        }
    }
}