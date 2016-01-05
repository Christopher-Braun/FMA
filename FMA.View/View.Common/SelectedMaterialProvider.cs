using System.Collections.Generic;
using System.Linq;
using FMA.View.Models;

namespace FMA.View
{
    public class SelectedMaterialProvider : NotifyPropertyChangedBase
    {
        private MaterialModel materialModel;
        private IMaterialChild selectedMaterialChild;

        public MaterialModel MaterialModel
        {
            get { return materialModel; }
            set
            {
                materialModel = value;
                SelectedMaterialChild = materialModel.MaterialFields.First();
                materialModel.PropertyChanged += (s, e) =>
                {
                    OnPropertyChanged(e.PropertyName);
                    OnPropertyChanged("MaterialChilds");
                };
                OnPropertyChanged();
            }
        }

        public IEnumerable<IMaterialChild> MaterialChilds
        {
            get
            {
                foreach (var materialField in MaterialModel.MaterialFields)
                {
                    yield return materialField;
                }

                if (MaterialModel.LogoModel.HasLogo)
                {
                    yield return MaterialModel.LogoModel;
                }
            }
        } 

        public IMaterialChild SelectedMaterialChild
        {
            get { return selectedMaterialChild; }
            set
            {
                selectedMaterialChild = value;
                OnPropertyChanged();
            }
        }
}
}