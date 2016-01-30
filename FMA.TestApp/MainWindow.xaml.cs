using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace FMA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private object flyerMakerViewModel;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            var commandLineStrings = Environment.CommandLine.Split(' ');
            if (String.Equals(commandLineStrings.Last(), "admin", StringComparison.InvariantCultureIgnoreCase))
            {
                FlyerMakerViewModel = MainViewModelFactory.CreateAdminViewModel(this);
            }
            else
            {
                FlyerMakerViewModel = MainViewModelFactory.CreateFlyerViewModel(this);
            }
        }

        public object FlyerMakerViewModel
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
