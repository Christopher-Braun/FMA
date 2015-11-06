using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FMA.Contracts;

namespace FMA.Core
{
    public class FlyerCreator
    {
        public static MemoryStream CreateFlyer(CustomMaterial material)
        {
            var image = CreateImage(material);

            return SaveDrawingImage(image);
        }

        public static DrawingImage CreateImage(CustomMaterial material)
        {
            var bitmapSourceBackground = material.GetFlyerBackground();
                 var flyerLogo = material.GetFlyerLogo();

            var visual = new DrawingVisual();
            using (var drawingContext = visual.RenderOpen())
            {
                drawingContext.DrawImage(bitmapSourceBackground, new Rect(0, 0, bitmapSourceBackground.Width, bitmapSourceBackground.Height));

                foreach (var textField in material.GetTextFields())
                {
                    drawingContext.DrawText(textField.FormattedText, textField.Origin);
                }
           
                if(flyerLogo != null)
                {
                    drawingContext.DrawImage(flyerLogo,new Rect(material.LogoLeftMargin, material.LogoTopMargin, material.LogoWidth, material.LogoHeight));
                }

            }
            var image = new DrawingImage(visual.Drawing);
            return image;
        }

        private static MemoryStream SaveDrawingImage(DrawingImage drawingImage)
        {
            var image = new Image
            {
                Source = drawingImage
            };

            image.Arrange(new Rect(0, 0, drawingImage.Width, drawingImage.Height));

            var renderTargetBitmap = new RenderTargetBitmap((int)drawingImage.Width, (int)drawingImage.Height, 96, 96, PixelFormats.Default);
            renderTargetBitmap.Render(image);

            var encoder = new JpegBitmapEncoder();

            encoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

            var memoryStream = new MemoryStream();

            encoder.Save(memoryStream);

            return memoryStream;
        }
    }
}
