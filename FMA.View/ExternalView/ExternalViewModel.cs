using FMA.Contracts;
using FMA.View.Models;
using FMA.View.View.Common;

namespace FMA.View.ExternalView
{
    public class ExternalViewModel : NotifyPropertyChangedBase
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

        public ExternalViewModel(SelectedMaterialProvider selectedMaterialProvider, FontService fontService)
        {
            this.SelectedMaterialProvider = selectedMaterialProvider;
            FontService = fontService;
        }
    }
}