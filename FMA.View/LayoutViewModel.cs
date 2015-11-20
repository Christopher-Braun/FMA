namespace FMA.View
{
    public class LayoutViewModel : FlyerViewModelBase
    {
        public LayoutViewModel(SelectedMaterialProvider selectedMaterialProvider, bool previewVisible, bool inputVisible, bool bothVisible) 
            : base(selectedMaterialProvider, previewVisible, inputVisible, bothVisible)
        {
        }

        public override bool CanCreate
        {
            get { return true; }
        }
    }
}
