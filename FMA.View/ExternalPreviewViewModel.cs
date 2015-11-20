namespace FMA.View
{
    public class ExternalPreviewViewModel
    {
        public SelectedMaterialProvider SelectedMaterialProvider { get; private set; }

        public ExternalPreviewViewModel(SelectedMaterialProvider selectedMaterialProvider)
        {
            this.SelectedMaterialProvider = selectedMaterialProvider;
        }
    }
}