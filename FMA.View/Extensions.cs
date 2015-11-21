using System.Linq;
using System.Windows;
using FMA.View.Models;

namespace FMA.View
{
    public static class Extensions
    {
        public static bool DropLogo(this FrameworkElement frameworkElement,MaterialModel materialModel, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                return false;
            }

            var files = (string[]) e.Data.GetData(DataFormats.FileDrop);

            var logoFile = files.First();

            var position = e.GetPosition(frameworkElement);

            materialModel.SetLogo(logoFile, position);
            return true;
        }
    }
}