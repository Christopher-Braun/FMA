using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using Microsoft.Win32;

namespace FMA.View.Models
{
    public static class LogoExtensions
    {
        public static void AddLogo(this MaterialModel materialModel)
        {
            var fileName = ShowFileOpenDialog();

            materialModel.SetLogo(fileName, new Point(25, 75));
        }

        public static string ShowFileOpenDialog()
        {
            var dialog = new OpenFileDialog
            {
                //TODO Remove when going productive
                Title = "Select logo file",
                InitialDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
                Filter = "Image files | *.jpg; *.jpeg; *.bmp; *.png; *.gif | All Files | *.*"
            };

            var result = dialog.ShowDialog();
            if (result != true)
            {
                return string.Empty;
            }

            var fileName = dialog.FileName;
            return fileName;
        }

        public static bool DropLogo(this FrameworkElement frameworkElement, MaterialModel materialModel, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                return false;
            }

            var files = (string[])e.Data.GetData(DataFormats.FileDrop);

            var logoFile = files.First();

            var position = e.GetPosition(frameworkElement);

            materialModel.SetLogo(logoFile, position);
            return true;
        }

        public static void SetLogo(this MaterialModel materialModel, string logoFile, Point position)
        {
            var logoData = GetByteArrayFromFile(logoFile);

            var logoModel = materialModel.LogoModel;

            logoModel.Logo = logoData;

            if (logoModel.LogoImage == null)
            {
                //TODO besser machen
                logoModel.Logo = null;
                return;
            }

            var logoWidth = logoModel.LogoImage.PixelWidth;
            var logoHeight = logoModel.LogoImage.PixelHeight;

            var widthRatio = (materialModel.FlyerFrontSideImage.Width / 2) / logoWidth;
            var heigthRatio = (materialModel.FlyerFrontSideImage.Height / 2) / logoHeight;

            var minRatio = Math.Min(widthRatio, heigthRatio);

            if (minRatio > 1)
            {
                minRatio = 1; //Only want to scale down
            }

            logoModel.Width = logoWidth * minRatio;
            logoModel.Height = logoHeight * minRatio;


            logoModel.LeftMargin = (int)position.X;
            logoModel.TopMargin = (int)position.Y;
        }

        public static byte[] GetByteArrayFromFile(string logoFile)
        {
            byte[] logoData;

            using (var fileStream = new FileStream(logoFile, FileMode.Open))
            {
                using (var ms = new MemoryStream())
                {
                    fileStream.CopyTo(ms);
                    logoData = ms.ToArray();
                }
            }
            return logoData;
        }
    }
}
