using System.IO;
using System.Linq;
using System.Windows;
using FMA.Contracts;
using FMA.Core;
using FMA.TestData;
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

            var viewModel = this.FlyerMakerView.ViewModel;

            var dummyMaterials = DummyData.GetDummyMaterials();
            viewModel.SetMaterials(dummyMaterials, dummyMaterials.Last());

            viewModel.FlyerCreated += (cm) =>
            {
                var flyer = FlyerCreator.CreateFlyer(cm);

                using (var fileStream = new FileStream("TestAppFlyer.jpg", FileMode.Create))
                {
                    flyer.WriteTo(fileStream);
                }
            };
        }
    }
}
