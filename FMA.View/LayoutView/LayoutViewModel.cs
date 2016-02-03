// Christopher Braun 2016

using System.Linq;
using FMA.Contracts;
using FMA.View.Common;
using FMA.View.Models;

namespace FMA.View.LayoutView
{
    public class LayoutViewModel : FlyerViewModelBase
    {
        public LayoutViewModel(SelectedMaterialProvider selectedMaterialProvider, IFontService fontService,
            ViewStates viewState)
            : base(selectedMaterialProvider, fontService, viewState)
        {
        }


        public override bool CanCreate => true;

        private MaterialModel MaterialModel => SelectedMaterialProvider.MaterialModel;

        public void AddField()
        {
            MaterialModel.AddNewMaterialField();
            SelectedMaterialProvider.SetSelectedChilds(MaterialModel.MaterialFields.Last());
        }

        public void DeleteSelectedChild()
        {
            var index = int.MaxValue;
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

                    
                    var currentIndex = materialFieldModels.IndexOf(selectedFieldModel);
                    if (currentIndex == -1)
                    {
                        return;
                    }

                    if (currentIndex < index)
                    {
                        index = currentIndex;
                    }

                    materialFieldModels.Remove(selectedFieldModel);
                }
            }

            if (materialFieldModels.Count > index)
            {
                SelectedMaterialProvider.SetSelectedChilds(materialFieldModels[index]);
            }
            else if (materialFieldModels.Any())
            {
                SelectedMaterialProvider.SetSelectedChilds(materialFieldModels[materialFieldModels.Count - 1]);
            }
            else
            {
                SelectedMaterialProvider.SetSelectedChilds();
            }
        }
    }
}