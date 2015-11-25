using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
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
            var image = CreateImage(material, CancellationToken.None);

            return SaveDrawingImage(image);
        }

        public static ImageSource CreateImage(CustomMaterial material, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var bitmapSourceBackground = material.GetFlyerBackground();

            ct.ThrowIfCancellationRequested();

            var visual = new DrawingVisual();
            using (var drawingContext = visual.RenderOpen())
            {
                ct.ThrowIfCancellationRequested();

                drawingContext.DrawImage(bitmapSourceBackground, new Rect(0, 0, bitmapSourceBackground.Width, bitmapSourceBackground.Height));

                ct.ThrowIfCancellationRequested();

                foreach (var textField in material.GetTextFields())
                {
                    ct.ThrowIfCancellationRequested();
                    drawingContext.DrawText(textField.FormattedText, textField.Origin);
                }

                if (material.CustomLogo.HasLogo)
                {
                    ct.ThrowIfCancellationRequested();
                    var flyerLogo = material.GetFlyerLogo();
                    drawingContext.DrawImage(flyerLogo, new Rect(material.CustomLogo.LogoLeftMargin, material.CustomLogo.LogoTopMargin, material.CustomLogo.LogoWidth, material.CustomLogo.LogoHeight));
                }

                ct.ThrowIfCancellationRequested();

            }
            var image = new DrawingImage(visual.Drawing);
            image.Freeze();
            return image;
        }

        private static MemoryStream SaveDrawingImage(ImageSource drawingImage)
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
