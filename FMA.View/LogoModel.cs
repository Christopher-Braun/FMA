using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media.Imaging;
using FMA.Core;
using FMA.View.Annotations;

namespace FMA.View
{
    public class LogoModel : INotifyPropertyChanged
    {
        private byte[] logo;
        private int leftMargin;
        private int topMargin;
        private Size size = Size.Empty;

        public LogoModel()
        {
            
        }

        public LogoModel(byte[] logo, int leftMargin, int topMargin)
        {
            this.logo = logo;
            this.leftMargin = leftMargin;
            this.topMargin = topMargin;
        }

        public bool HasLogo
        {
            get { return Logo != null && Logo.Length > 0; }
        }

        public BitmapImage LogoImage
        {
            get { return this.Logo.GetBitmapImage(); }
        }

        public Byte[] Logo
        {
            get { return logo; }
            set
            {
                logo = value;
                OnPropertyChanged();
            }
        }

        public int LeftMargin
        {
            get { return leftMargin; }
            set
            {
                leftMargin = value;
                OnPropertyChanged();
            }
        }

        public int TopMargin
        {
            get { return topMargin; }
            set
            {
                topMargin = value; 
                OnPropertyChanged();
            }
        }

        public Size Size
        {
            get
            {
                if (this.HasLogo == false)
                {
                    return Size.Empty;
                }
                else if (size == Size.Empty)
                {
                    var logoImage = this.LogoImage;
                    return new Size(logoImage.Width, logoImage.Height);
                }
                return size;
            }
            set
            {
                size = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}