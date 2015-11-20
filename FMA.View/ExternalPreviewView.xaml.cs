using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using FMA.View.Annotations;

namespace FMA.View
{
    /// <summary>
    /// Interaction logic for ExternalPreviewView.xaml
    /// </summary>
    public partial class ExternalPreviewView : Window, INotifyPropertyChanged
    {
        private readonly Func<ImageSource> getPreview;

        public ExternalPreviewView(Func<ImageSource> getPreview)
        {

            this.getPreview = getPreview;
            InitializeComponent();
            this.DataContext = this;

            const int margin = 40;
            this.Top = margin;
            this.Height = SystemParameters.PrimaryScreenHeight - 2 * margin;
        }

        public ImageSource FlyerPreview
        {
            get
            {
                return getPreview();
            }
        }

        public void FlyerChanged()
        {
            this.OnPropertyChanged("FlyerPreview");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void FlyerPreviewView_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void FlyerPreviewView_OnSourceInitialized(object sender, EventArgs e)
        {
            WindowAspectRatio.Register((Window)sender);
        }
    }
}
