using System.ComponentModel;
using System.Runtime.CompilerServices;
using FMA.View.Properties;

namespace FMA.View.Models
{
    public class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        //private bool suspendRefreshPreview;
        //private bool missedEvents;

        public void SuspendNotifyPropertyChanged()
        {
      //      this.suspendRefreshPreview = true;
        }

        public void ResumeNotifyPropertyChanged(bool notifyMissedEvents = true,string propertyName = "")
        {
            //this.suspendRefreshPreview = false;
            //if (notifyMissedEvents && missedEvents)
            //{
            //        this.OnPropertyChanged(propertyName);
            //}

            //missedEvents = false;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            //if (suspendRefreshPreview)
            //{
            //    missedEvents = true;
            //    return;
            //}

            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}