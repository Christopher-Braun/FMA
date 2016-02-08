// Christopher Braun 2016

using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace FMA
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private object flyerMakerViewModel;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            var commandLineStrings = Environment.CommandLine.Split(' ');
            if (string.Equals(commandLineStrings.Last(), "admin", StringComparison.InvariantCultureIgnoreCase))
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
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}