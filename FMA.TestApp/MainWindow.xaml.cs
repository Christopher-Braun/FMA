using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using FMA.Contracts;
using FMA.Core;
using FMA.TestData;
using FMA.View;
using FMA.View.Common;
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

            FlyerMakerViewModel = MainViewModelFactory.CreateFlyerViewModel(this);
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
