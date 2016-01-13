using System;
using System.Windows;
using FMA.View.Helpers;

namespace FMA.View.ExternalView
{
    /// <summary>
    /// Interaction logic for ExternalPreviewView.xaml
    /// </summary>
    public partial class ExternalEditView : Window
    {
        public ExternalEditView()
        {
            InitializeComponent();

            const int margin = 40;
            this.Top = margin;
            this.Height = SystemParameters.PrimaryScreenHeight - 2 * margin;
        }

        private void FlyerPreviewView_OnSourceInitialized(object sender, EventArgs e)
        {
            WindowAspectRatio.Register((Window)sender);
        }
    }
}
