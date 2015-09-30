using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FMA.Core
{
    public class ImageSaver
    {
        public static void SaveDrawingImage(DrawingImage drawingImage, string filePath)
        {
            using (var memoryStream = SaveDrawingImage(drawingImage))
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    memoryStream.WriteTo(fileStream);
                }
            }
        }

        public static MemoryStream SaveDrawingImage(DrawingImage drawingImage)
        {
            var image = new Image
            {
                Source = drawingImage
            };

            image.Arrange(new Rect(0, 0, drawingImage.Width, drawingImage.Height));

            var rtb = new RenderTargetBitmap((int) drawingImage.Width, (int) drawingImage.Height, 96, 96,
                PixelFormats.Default);
            rtb.Render(image);

            var encoder = new JpegBitmapEncoder();

            encoder.Frames.Add(BitmapFrame.Create(rtb));

            var memoryStream = new MemoryStream();

            encoder.Save(memoryStream);

            return memoryStream;
        }
    }
}