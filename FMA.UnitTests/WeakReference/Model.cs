using System.ComponentModel;
using System.Runtime.CompilerServices;
using FMA.UnitTests.Properties;

namespace FMA.UnitTests.WeakReference
{
    public class Model : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public void FirePropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }
    }
}