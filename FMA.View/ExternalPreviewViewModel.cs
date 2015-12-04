using FMA.View.Models;

namespace FMA.View
{
    public class ExternalPreviewViewModel : NotifyPropertyChangedBase
    {
        private bool canEdit;
        private SelectedMaterialProvider selectedMaterialProvider;

        public SelectedMaterialProvider SelectedMaterialProvider
        {
            get { return selectedMaterialProvider; }
            private set
            {
                if (Equals(value, selectedMaterialProvider)) return;
                selectedMaterialProvider = value;
                OnPropertyChanged();
            }
        }

        public bool CanEdit
        {
            get { return canEdit; }
            private set
            {
                if (value.Equals(canEdit)) return;
                canEdit = value;
                OnPropertyChanged();
            }
        }

        public ExternalPreviewViewModel(SelectedMaterialProvider selectedMaterialProvider, bool canEdit)
        {
            CanEdit = canEdit;
            this.SelectedMaterialProvider = selectedMaterialProvider;
        }
    }
}