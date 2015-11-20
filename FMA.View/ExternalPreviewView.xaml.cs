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
    public partial class ExternalPreviewView : Window
    {
        public SelectedMaterialProvider SelectedMaterialProvider { get; private set; }

        public ExternalPreviewView(SelectedMaterialProvider selectedMaterialProvider)
        {
            this.SelectedMaterialProvider = selectedMaterialProvider;
           
            InitializeComponent();
            this.DataContext = this;

            const int margin = 40;
            this.Top = margin;
            this.Height = SystemParameters.PrimaryScreenHeight - 2 * margin;
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
