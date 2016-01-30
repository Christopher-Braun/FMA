using System.Drawing;
using System.IO;

namespace FMA.AdminView
{
    public static class ImageHelper
    {
        public static byte[] ToByteArray(this Image image)
        {
            if (image == null)
            {
                return new byte[0];
            }

            byte[] byteArray;
            using (var stream = new MemoryStream())
            {
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                stream.Close();
                byteArray = stream.ToArray();
            }
            return byteArray;
        }
    }
}