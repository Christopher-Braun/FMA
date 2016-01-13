using FMA.Contracts;
using FMA.View.Common;
using FMA.View.Models;

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

        public IFontService FontService { get; set; }

        public ExternalViewModel(SelectedMaterialProvider selectedMaterialProvider, IFontService fontService)
        {
            this.SelectedMaterialProvider = selectedMaterialProvider;
            FontService = fontService;
        }
    }
}