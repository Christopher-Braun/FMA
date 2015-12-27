using System.Linq;
using FMA.Contracts;
using FMA.View.Models;

namespace FMA.View
{
    public class LayoutViewModel : FlyerViewModelBase
    {
        public LayoutViewModel(SelectedMaterialProvider selectedMaterialProvider, FontService fontService, ViewStates viewState)
            : base(selectedMaterialProvider, fontService, viewState)
        {
        }

        public override bool CanCreate
        {
            get { return true; }
        }

        public void AddField()
        {
          

            this.SelectedMaterialProvider.MaterialModel.AddMaterialField();
        }
    }
}
