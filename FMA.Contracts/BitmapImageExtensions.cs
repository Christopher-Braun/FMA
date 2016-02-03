using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace FMA.Contracts
{
    public static class BitmapImageExtensions
    {
        public static BitmapImage GetBitmapImage(this byte[] bytes)
        {
            var imageSource = new BitmapImage();
            try
            {
                if (bytes == null || bytes.Length == 0)
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
            }
            catch (Exception)
            {
                //Log
            }

            return imageSource;
        }


    }
}