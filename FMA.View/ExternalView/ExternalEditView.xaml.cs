// Christopher Braun 2016

using System;

namespace FMA.View.ExternalView
{
    /// <summary>
    ///     Interaction logic for ExternalPreviewView.xaml
    /// </summary>
    public partial class ExternalEditView
    {
        private readonly SizeHelper sizeHelper;

        public ExternalEditView()
        {
            InitializeComponent();
            sizeHelper = new SizeHelper(this);
        }

        private void FlyerPreviewView_OnSourceInitialized(object sender, EventArgs e)
        {
            sizeHelper.SetSize();
        }
    }
}