using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

namespace FMA.Contracts
{
    public static class BitmapImageExtensions
    {
        public static BitmapImage GetBitmapImage(this byte[] bytes)
        {
            var imageSource = new BitmapImage();
            if (bytes == null || bytes.Count() == 0)
            {
                return imageSource;
            }

            using (var stream = new MemoryStream(bytes))
            {
                stream.Seek(0, SeekOrigin.Begin);
                imageSource.BeginInit();
                imageSource.StreamSource = stream;
                imageSource.CacheOption = BitmapCacheOption.OnLoad;
                imageSource.EndInit();
            }

            return imageSource;
        }
    }
}