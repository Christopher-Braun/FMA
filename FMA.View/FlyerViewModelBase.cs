using System.ComponentModel;
using System.Runtime.CompilerServices;
using FMA.View.Annotations;

namespace FMA.View
{
    public abstract class FlyerViewModelBase : INotifyPropertyChanged
    {
        private bool previewVisible;
        private bool inputVisible;
        private bool bothVisible;

        protected FlyerViewModelBase(SelectedMaterialProvider selectedMaterialProvider, bool previewVisible, bool inputVisible, bool bothVisible)
        {
            SelectedMaterialProvider = selectedMaterialProvider;
            this.previewVisible = previewVisible;
            this.inputVisible = inputVisible;
            this.bothVisible = bothVisible;
        }

        public SelectedMaterialProvider SelectedMaterialProvider { get; private set; }

        public abstract bool CanCreate { get; }

        public bool PreviewVisible
        {
            get { return previewVisible; }
            set
            {
                if (value == previewVisible) return;
                previewVisible = value;
                OnPropertyChanged();
            }
        }

        public bool InputVisible
        {
            get { return inputVisible; }
            set
            {
                if (value == inputVisible) return;
                inputVisible = value;
                OnPropertyChanged();
            }
        }

        public bool BothVisible
        {
            get { return bothVisible; }
            set
            {
                if (value.Equals(bothVisible)) return;
                bothVisible = value;
                OnPropertyChanged();
            }
        }

        public virtual event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}