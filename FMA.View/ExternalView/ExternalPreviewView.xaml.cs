// Christopher Braun 2016

using System;
using System.Windows;
using System.Windows.Input;
using FMA.View.Helpers;

namespace FMA.View.ExternalView
{
    /// <summary>
    ///     Interaction logic for ExternalPreviewView.xaml
    /// </summary>
    public partial class ExternalPreviewView
    {
        private readonly SizeHelper sizeHelper;

        public ExternalPreviewView()
        {
            InitializeComponent();
            sizeHelper = new SizeHelper(this);
        }

        private void FlyerPreviewView_OnSourceInitialized(object sender, EventArgs e)
        {
            WindowAspectRatio.Register((Window) sender);
            sizeHelper.SetSize();
        }

        private void ExternalPreviewView_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
            e.Handled = true;
        }

        private void ExternalPreviewView_OnPreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Close();
            e.Handled = true;
        }
    }
}