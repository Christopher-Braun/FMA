using System.ComponentModel;
using System.Runtime.CompilerServices;
using FMA.View.Annotations;

namespace FMA.View.Models
{
    public class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        private bool suspendNotify;
        private bool propertyWasChanged;

        public void SuspendNotifyPropertyChanged()
        {
            this.suspendNotify = true;
        }

        public void ResumeNotifyPropertyChanged(bool notifyMissedEvents =true)
        {
            this.suspendNotify = false;
            if (propertyWasChanged && notifyMissedEvents)
            {
                this.OnPropertyChanged("Maybe Many");
            }

            this.propertyWasChanged = false;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (suspendNotify)
            {
                propertyWasChanged = true;
                return;
            }

            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}