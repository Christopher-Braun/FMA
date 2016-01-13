using System;
using System.Collections.Generic;
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

        public void AddField()
        {
            MaterialModel.AddNewMaterialField();
            this.SelectedMaterialProvider.SetSelectedChilds(MaterialModel.MaterialFields.Last());
        }

        public void DeleteSelectedChild()
        {
            int index = int.MaxValue;
            var materialFieldModels = MaterialModel.MaterialFields;

            foreach (var selectedChild in SelectedMaterialProvider.SelectedMaterialChilds.ToArray())
            {
                if (selectedChild is LogoModel)
                {
                    MaterialModel.LogoModel.DeleteLogo();
                }
                else
                {
                    var selectedFieldModel = selectedChild as MaterialFieldModel;

                    index = materialFieldModels.IndexOf(selectedFieldModel);

                    if (index == -1)
                    {
                        return;
                    }

                    materialFieldModels.Remove(selectedFieldModel);
                }
            }

            if (materialFieldModels.Count > index)
            {
                this.SelectedMaterialProvider.SetSelectedChilds(materialFieldModels[index]);
            }
            else if (materialFieldModels.Any())
            {
                this.SelectedMaterialProvider.SetSelectedChilds(materialFieldModels[materialFieldModels.Count - 1]);
            }
            else
            {
                this.SelectedMaterialProvider.SetSelectedChilds();
            }
        }
    }
}
