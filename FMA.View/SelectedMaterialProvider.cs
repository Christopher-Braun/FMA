using System.Linq;
using FMA.View.Models;

namespace FMA.View
{
    public class SelectedMaterialProvider : NotifyPropertyChangedBase
    {
        private MaterialModel materialModel;
        private MaterialFieldModel selectedMaterialField;

        public MaterialModel MaterialModel
        {
            get { return materialModel; }
            set
            {
                materialModel = value;
                SelectedMaterialField = materialModel.MaterialFields.First();
                materialModel.PropertyChanged += (s, e) => OnPropertyChanged(e.PropertyName);
                OnPropertyChanged();
            }
        }

        public MaterialFieldModel SelectedMaterialField
        {
            get { return selectedMaterialField; }
            set
            {
                selectedMaterialField = value;
                OnPropertyChanged();
            }
        }


}
}