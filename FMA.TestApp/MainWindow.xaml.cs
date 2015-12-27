using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using FMA.Core;
using FMA.TestData;
using FMA.View;
using FMA.View.Helpers;

namespace FMA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private FlyerMakerViewModel flyerMakerViewModel;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            var dummyMaterials = DummyData.GetDummyMaterials();

            //Dummy wird später ersetzt durch WCF Connection
            Func<string, FontInfo> getFont = name =>
            {
                if (name == "Signarita Anne")
                {
                    return new FontInfo("SignaritaAnne.ttf", Helper.GetByteArrayFromFile("SignaritaAnneDemo.ttf"));
                }
                if (name == "Bakery")
                {
                    return new FontInfo("Bakery.ttf", Helper.GetByteArrayFromFile("bakery.ttf"));
                }

                return null;
            };

            var exeDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var customFontsDir = string.Format(@"{0}\CustomFonts\", exeDir);
            var viewModel = new FlyerMakerViewModel(dummyMaterials, 2, getFont, customFontsDir);
            var flyerCreator = new FlyerCreator(customFontsDir);

            viewModel.WindowService = new WindowService(this);

            viewModel.FlyerCreated += cm =>
            {
                var flyer = flyerCreator.CreateFlyer(cm);

                var flyerJpg = "TestAppFlyer.jpg";
                using (var fileStream = new FileStream(flyerJpg, FileMode.Create))
                {
                    flyer.WriteTo(fileStream);
                    Process.Start(flyerJpg);
                }
            };

            FlyerMakerViewModel = viewModel;
        }

        public FlyerMakerViewModel FlyerMakerViewModel
        {
            get { return flyerMakerViewModel; }
            set
            {
                if (Equals(value, flyerMakerViewModel)) return;
                flyerMakerViewModel = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
