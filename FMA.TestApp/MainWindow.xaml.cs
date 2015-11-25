using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using FMA.Core;
using FMA.TestData;
using FMA.View;
using FMA.View.Annotations;

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
            var viewModel = new FlyerMakerViewModel(dummyMaterials, 2);

            viewModel.WindowService = new WindowService(this);

            viewModel.FlyerCreated += (cm) =>
            {
                var flyer = FlyerCreator.CreateFlyer(cm);

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

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
