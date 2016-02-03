using System.IO;
using System.Reflection;
using System.Windows;
using FMA.Core;
using FMA.TestData;
using System.Diagnostics;

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
            var customFontsDir = $@"{exeDir}\CustomFonts\";

            var flyer = new FlyerCreator(customFontsDir).CreateFlyer(material);

            using (var fileStream = new FileStream("SilentFlyer.jpg", FileMode.Create))
            {
                flyer.WriteTo(fileStream);
            }

            Process.Start("SilentFlyer.jpg");
        }
    }
}
