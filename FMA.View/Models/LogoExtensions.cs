using System;
using System.Windows;
using FMA.Contracts;
using FMA.View.Helpers;

namespace FMA.View.Models
{
    public static class LogoExtensions
    {
        public static void AddLogo(this MaterialModel materialModel)
        {
            var fileName = FileHelper.ShowFileOpenDialogForImages();
            if (string.IsNullOrEmpty(fileName))
            {
                return;
            }
            materialModel.SetLogo(fileName, new Point(25, 75));
        }



        public static void SetLogo(this MaterialModel materialModel, string logoFile, Point position)
        {
            var logoData = FileHelper.GetByteArrayFromFile(logoFile);


            var image = logoData.GetBitmapImage();
            if (image == null)
            {
                return;
            }

            var logoModel = materialModel.LogoModel;
            logoModel.Logo = logoData;

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
    }
}
