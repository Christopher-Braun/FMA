using System.Windows;
using System.Windows.Controls;

namespace FMA.View.LayoutView
{
    /// <summary>
    /// Interaction logic for LayoutArea.xaml
    /// </summary>
    public partial class GraphicalLayout : UserControl
    {
        public GraphicalLayout()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            this.LayoutCanvas.DeleteSelectedElements();
        }
    }
}
