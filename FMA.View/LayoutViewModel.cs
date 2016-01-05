using System.Linq;
using FMA.Contracts;
using FMA.View.Models;

namespace FMA.View
{
    public class LayoutViewModel : FlyerViewModelBase
    {
        public LayoutViewModel(SelectedMaterialProvider selectedMaterialProvider, FontService fontService,
            ViewStates viewState)
            : base(selectedMaterialProvider, fontService, viewState)
        {
        }

        public override bool CanCreate
        {
            get { return true; }
        }

        private MaterialModel MaterialModel
        {
            get { return this.SelectedMaterialProvider.MaterialModel; }
        }

        private MaterialFieldModel SelectedFieldModel
        {
            get { return SelectedMaterialProvider.SelectedMaterialField; }
            set { this.SelectedMaterialProvider.SelectedMaterialField = value; }
        }

        public void AddField()
        {
            MaterialModel.AddMaterialField();
            SelectedFieldModel = MaterialModel.MaterialFields.Last();
        }

        public void DeleteSelectedField()
        {
            var materialFieldModels = MaterialModel.MaterialFields;

            var index = materialFieldModels.IndexOf(SelectedFieldModel);
            if (index == -1)
            {
                return;
            }

            materialFieldModels.Remove(SelectedFieldModel);

            if (materialFieldModels.Count > index)
            {
                SelectedFieldModel = materialFieldModels[index];
            }
            else if (materialFieldModels.Any())
            {
                SelectedFieldModel = materialFieldModels[materialFieldModels.Count - 1];
            }
            else
            {
                SelectedFieldModel = null;
            }
        }
    }
}
