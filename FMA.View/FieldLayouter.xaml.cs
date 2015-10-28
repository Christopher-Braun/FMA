using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FMA.View
{
    /// <summary>
    /// Interaction logic for FieldLayouter.xaml
    /// </summary>
    public partial class FieldLayouter : UserControl
    {
        public FieldLayouter()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty MaterialFieldModelProperty = DependencyProperty.Register(
            "MaterialFieldModel", typeof (MaterialFieldModel), typeof (FieldLayouter), new PropertyMetadata(default(MaterialFieldModel)));

        public MaterialFieldModel MaterialFieldModel
        {
            get { return (MaterialFieldModel) GetValue(MaterialFieldModelProperty); }
            set { SetValue(MaterialFieldModelProperty, value); }
        }
    }
}
