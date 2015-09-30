using System.Linq;
using System.Windows;
using FMA.Contracts;
using FMA.View;

namespace FMA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var viewModel = (this.FlyerMakerView.DataContext as IFlyerMaker);

            var dummyMaterials = DummyData.GetDummyMaterials();
            viewModel.SetMaterials(dummyMaterials);
            viewModel.SetSelectedMaterial(dummyMaterials.First());

           //var materials = new List<Material>();

            //var material1Fields = new List<MaterialField>
            //{
            //    new MaterialField("Referent", "Arial", 10, false, false, false, 50, 1, 7, 71, "Paulus von Tarsus"), 
            //    new MaterialField("Titel", "Arial", 21, true, false, true, 50, 3, 7, 20, "Aleppo")
            //};

            //var material1 = new Material(1, "Gefährlicher Glaube", "ZumThema Gefährlicher Glaube (Islamische Welt)", material1Fields);

            //materials.Add(material1);

            //viewModel.Materials = materials;
            //viewModel.SelectedMaterial = material1;
        }



    }
}
