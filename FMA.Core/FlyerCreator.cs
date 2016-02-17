using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FMA.Contracts;
using FMA.Core.Properties;

namespace FMA.Core
{
    public class FlyerCreator
    {
        private readonly MaterialToTextFieldConverter materialToTextFieldConverter;

        public FlyerCreator(string customFontsDir)
        {
            materialToTextFieldConverter = new MaterialToTextFieldConverter(new FontService(customFontsDir));
        }

        public MemoryStream CreateFlyer(CustomMaterial material)
        {
            var image = CreateImage(material);

            return SaveDrawingImage(image);
        }

        private ImageSource CreateImage(CustomMaterial material)
        {
            var bitmapSourceBackground = material.FlyerFrontSide.ToBitmapImage();

            var visual = new DrawingVisual();
            using (var drawingContext = visual.RenderOpen())
            {
                drawingContext.PushClip(new RectangleGeometry(new Rect(0, 0, bitmapSourceBackground.Width, bitmapSourceBackground.Height)));

                drawingContext.DrawImage(bitmapSourceBackground, new Rect(0, 0, bitmapSourceBackground.Width, bitmapSourceBackground.Height));

                foreach (var textField in material.MaterialFields.Select(materialToTextFieldConverter.CreateTextField))
                {
                    drawingContext.DrawText(textField.FormattedText, textField.Origin);
                }

                if (material.CustomLogo.HasLogo)
                {
                    var flyerLogo = material.CustomLogo.Logo.ToBitmapImage();
                    drawingContext.DrawImage(flyerLogo, new Rect(material.CustomLogo.LogoLeftMargin, material.CustomLogo.LogoTopMargin, material.CustomLogo.LogoWidth, material.CustomLogo.LogoHeight));
                }
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

            var settings = Settings.Default;

            

            var renderTargetBitmap = new RenderTargetBitmap((int)drawingImage.Width, (int)drawingImage.Height, settings.ImageDPI, settings.ImageDPI, PixelFormats.Default);
            renderTargetBitmap.Render(image);

            Settings.Default.Save();

            var encoder = new JpegBitmapEncoder
            {
                QualityLevel = settings.JpgQuality
            };

            encoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

            var memoryStream = new MemoryStream();

            encoder.Save(memoryStream);

            return memoryStream;
        }
    }
}
