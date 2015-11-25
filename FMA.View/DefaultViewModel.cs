using System.Linq;
using System.Windows;

namespace FMA.View
{
    public class DefaultViewModel : FlyerViewModelBase
    {
        public DefaultViewModel(SelectedMaterialProvider selectedMaterialProvider, bool previewVisible, bool inputVisible, bool bothVisible)
            : base(selectedMaterialProvider, previewVisible, inputVisible, bothVisible)
        {
        }

        public override bool CanCreate
        {
            get
            {
                if (this.SelectedMaterialProvider.MaterialModel == null)
                {
                    return false;
                }

                return SelectedMaterialProvider.MaterialModel.MaterialFields.All(f => string.IsNullOrEmpty(f.Error));
            }
        }
    }
}
