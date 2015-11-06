using System.IO;
using System.Linq;
using System.Windows;
using FMA.Core;
using FMA.TestData;

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

            var material = DummyData.GetCustomMaterial();

            var flyer = FlyerCreator.CreateFlyer(material);

            using (var fileStream = new FileStream("SilentFlyer.jpg", FileMode.Create))
            {
                flyer.WriteTo(fileStream);
            }

        }
    }
}
