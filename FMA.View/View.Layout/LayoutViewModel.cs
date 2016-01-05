using System;
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

        private IMaterialChild SelectedMaterialChild
        {
            get { return SelectedMaterialProvider.SelectedMaterialChild; }
            set { this.SelectedMaterialProvider.SelectedMaterialChild = value; }
        }

        public void AddField()
        {
            MaterialModel.AddMaterialField();
            SelectedMaterialChild = MaterialModel.MaterialFields.Last();
        }

        public void DeleteSelectedChild()
        {
            int index = int.MaxValue;
            var materialFieldModels = MaterialModel.MaterialFields;

            if (SelectedMaterialChild is LogoModel)
            {
                MaterialModel.LogoModel.DeleteLogo();
            }
            else
            {
                var selectedFieldModel = SelectedMaterialChild as MaterialFieldModel;
 
                index = materialFieldModels.IndexOf(selectedFieldModel);

                if (index == -1)
                {
                    return;
                }

                materialFieldModels.Remove(selectedFieldModel);
            }

            if (materialFieldModels.Count > index)
            {
                SelectedMaterialChild = materialFieldModels[index];
            }
            else if (materialFieldModels.Any())
            {
                SelectedMaterialChild = materialFieldModels[materialFieldModels.Count - 1];
            }
            else
            {
                SelectedMaterialChild = null;
            }
        }
    }
}
