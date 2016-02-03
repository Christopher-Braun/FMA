// Christopher Braun 2016

using System.Linq;
using FMA.Contracts;
using FMA.View.Common;

namespace FMA.View.DefaultView
{
    public class DefaultViewModel : FlyerViewModelBase
    {
        public DefaultViewModel(SelectedMaterialProvider selectedMaterialProvider, IFontService fontService,
            ViewStates viewState)
            : base(selectedMaterialProvider, fontService, viewState)
        {
        }

        public override bool CanCreate
        {
            get
            {
                var materialModel = SelectedMaterialProvider.MaterialModel;
                return materialModel != null && materialModel.MaterialFields.All(f => string.IsNullOrEmpty(f.Error));
            }
        }
    }
}