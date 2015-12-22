using System.IO;
using System.Reflection;
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

            var exeDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var customFontsDir = string.Format(@"{0}\CustomFonts\", exeDir);

            var flyer = (new FlyerCreator(customFontsDir)).CreateFlyer(material);

            using (var fileStream = new FileStream("SilentFlyer.jpg", FileMode.Create))
            {
                flyer.WriteTo(fileStream);
            }

        }
    }
}
