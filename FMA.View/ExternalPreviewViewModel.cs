using FMA.Contracts;
using FMA.View.Models;

namespace FMA.View
{
    public class ExternalPreviewViewModel : NotifyPropertyChangedBase
    {
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

        public FontService FontService { get; set; }

        public ExternalPreviewViewModel(SelectedMaterialProvider selectedMaterialProvider, FontService fontService)
        {
            this.SelectedMaterialProvider = selectedMaterialProvider;
            FontService = fontService;
        }
    }
}