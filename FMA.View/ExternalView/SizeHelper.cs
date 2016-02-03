using System.Windows;

namespace FMA.View.ExternalView
{
    public class SizeHelper
    {
        private readonly Window externalWindow;

        private const int margin = 40;

        public SizeHelper(Window externalWindow)
        {
            this.externalWindow = externalWindow;
        }

        public void SetSize()
        {
            externalWindow.Top = margin;
            externalWindow.Left = margin;
            externalWindow.Height = SystemParameters.PrimaryScreenHeight - 2*margin;
            var maxWidth = SystemParameters.PrimaryScreenWidth / 2;

            var ratio = maxWidth/externalWindow.ActualWidth;

            if (ratio < 1)
            {
                externalWindow.Height *= ratio;
            }
        }
    }
}