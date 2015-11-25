using System;
using System.Windows.Media.Imaging;
using FMA.Core;

namespace FMA.View.Models
{
    public class LogoModel : NotifyPropertyChangedBase
    {
        private byte[] logo;
        private double leftMargin;
        private double topMargin;
        private double height;
        private double width;
        private BitmapImage logoImage;

        public bool HasLogo
        {
            get { return Logo != null && Logo.Length > 0; }
        }

        public BitmapImage LogoImage
        {
            get { return logoImage; }
            private set
            {
                logoImage = value;
                OnPropertyChanged();
            }
        }

        public Byte[] Logo
        {
            get { return logo; }
            set
            {
                logo = value;
                this.LogoImage = this.Logo.GetBitmapImage();
                OnPropertyChanged();
                OnPropertyChanged("HasLogo");
            }
        }

        public double LeftMargin
        {
            get { return leftMargin; }
            set
            {
                leftMargin = value;
                OnPropertyChanged();
            }
        }

        public double TopMargin
        {
            get { return topMargin; }
            set
            {
                topMargin = value;
                OnPropertyChanged();
            }
        }

        public double Width
        {
            get { return width; }
            set
            {
                width = value;
                OnPropertyChanged();
            }
        }
        public double Height
        {
            get { return height; }
            set
            {
                height = value;
                OnPropertyChanged();
            }
        }

     

     
    }
}