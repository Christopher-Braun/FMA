using System.Linq;
using System.Windows;
using FMA.Core;

namespace FMA.SilentMode.TestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var material = DummyData.GetDummyMaterials().First();


            FlyerMaker.CreateFlyer(material, "SilentFlyer.jpg");
        }
    }
}
