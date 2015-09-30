using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FMA.Contracts;

namespace FMA.Core
{
    public class ImageCreator
    {
        public static DrawingImage CreateImagePreview(CustomMaterial material)
        {
            return ImageCreator.CreateImagePreview(material.GetFlyerBackground(), material.GetTextFields());
        }

        public static DrawingImage CreateImagePreview(BitmapSource bitmapSource, IEnumerable<TextField> texts)
        {
            var visual = new DrawingVisual();
            using (var drawingContext = visual.RenderOpen())
            {
                drawingContext.DrawImage(bitmapSource, new Rect(0, 0, bitmapSource.Width, bitmapSource.Height));

                foreach (var textField in texts)
                {
                    drawingContext.DrawText(textField.FormattedText, textField.Origin);
                }
       
            }
            var image = new DrawingImage(visual.Drawing);
            return image;
        }
    }
}