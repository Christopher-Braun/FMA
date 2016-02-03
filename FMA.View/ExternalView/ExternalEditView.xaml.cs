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
        private readonly SizeHelper sizeHelper;

        public ExternalEditView()
        {
            InitializeComponent();
            sizeHelper = new SizeHelper(this);
        }

        private void FlyerPreviewView_OnSourceInitialized(object sender, EventArgs e)
        {
         //   WindowAspectRatio.Register((Window)sender);
            sizeHelper.SetSize();
        }
    }
}
