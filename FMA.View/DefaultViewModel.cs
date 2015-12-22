using System;
using System.Linq;
using FMA.Contracts;

namespace FMA.View
{
    public class DefaultViewModel : FlyerViewModelBase
    {
        public DefaultViewModel(SelectedMaterialProvider selectedMaterialProvider, FontService fontService, ViewStates viewState)
            : base(selectedMaterialProvider,fontService, viewState)
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
