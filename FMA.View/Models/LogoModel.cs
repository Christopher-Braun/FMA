using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using FMA.Core;
using FMA.View.Annotations;

namespace FMA.View
{
    public class LogoModel : INotifyPropertyChanged
    {
        private byte[] logo;
        private double leftMargin;
        private double topMargin;
        private double height;
        private double width;

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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}