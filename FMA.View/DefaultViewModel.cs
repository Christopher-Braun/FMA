using System.Linq;
using System.Windows;

namespace FMA.View
{
    public class DefaultViewModel: FlyerViewModelBase
    {
        public DefaultViewModel(SelectedMaterialProvider selectedMaterialProvider, bool previewVisible, bool inputVisible, bool bothVisible) 
            : base(selectedMaterialProvider, previewVisible, inputVisible, bothVisible)
        {
        }

        public override bool CanCreate
        {
            get
            {
                if (this.SelectedMaterialProvider.SelectedMaterial == null)
                {
                    return false;
                }

                return SelectedMaterialProvider.SelectedMaterial.MaterialFields.All(f => string.IsNullOrEmpty(f.Error));
            }
        }

        public void OnDrop(object sender, DragEventArgs e)
        {
            var frameworkElement = sender as FrameworkElement;
            frameworkElement.DropLogo(this.SelectedMaterialProvider.SelectedMaterial, e);
        }

    }
}
