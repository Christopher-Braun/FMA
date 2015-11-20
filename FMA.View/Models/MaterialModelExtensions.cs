using System;
using System.IO;
using System.Windows;

namespace FMA.View
{
    public static class MaterialModelExtensions
    {
        public static void SetLogo(this MaterialModel materialModel, string logoFile, Point position)
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

            var logoModel = materialModel.LogoModel;
           
            logoModel.SuspendNotifyPropertyChanged();
            try
            {
                logoModel.Logo = logoData;

                var logoWidth = logoModel.LogoImage.PixelWidth;
                var logoHeight = logoModel.LogoImage.PixelHeight;

                var widthRatio = (materialModel.FlyerFrontSideImage.Width/2)/logoWidth;
                var heigthRatio = (materialModel.FlyerFrontSideImage.Height/2)/logoHeight;

                var minRatio = Math.Min(widthRatio, heigthRatio);

                if (minRatio > 1)
                {
                    minRatio = 1; //Only want to scale down
                }

                logoModel.Width = logoWidth*minRatio;
                logoModel.Height = logoHeight*minRatio;


                logoModel.LeftMargin = (int) position.X;
                logoModel.TopMargin = (int) position.Y;
            }
            finally
            {
                logoModel.ResumeNotifyPropertyChanged();
            }
        }
    }
}