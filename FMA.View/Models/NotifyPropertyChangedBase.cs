using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Documents;
using FMA.View.Annotations;

namespace FMA.View.Models
{
    public class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        //private bool suspendRefreshPreview;
        //private List<string> changedProperties = new List<string>();

        //public void SuspendNotifyPropertyChanged()
        //{
        //    this.suspendRefreshPreview = true;
        //}

        //public void ResumeNotifyPropertyChanged(bool notifyMissedEvents = true)
        //{
        //    this.suspendRefreshPreview = false;
        //    if (notifyMissedEvents)
        //    {
        //        foreach (var propertyName in changedProperties)
        //        {
        //            this.OnPropertyChanged(propertyName);
        //        }
        //    }

        //    this.changedProperties.Clear();
        //}


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            //if (suspendRefreshPreview)
            //{
            //    changedProperties.Add(propertyName);
            //    return;
            //}

            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}