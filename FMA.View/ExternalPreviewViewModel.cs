using FMA.View.Models;

namespace FMA.View
{
    public class ExternalPreviewViewModel : NotifyPropertyChangedBase
    {
        public SelectedMaterialProvider SelectedMaterialProvider { get; private set; }

        public ExternalPreviewViewModel(SelectedMaterialProvider selectedMaterialProvider)
        {
            this.SelectedMaterialProvider = selectedMaterialProvider;
        }
    }
}