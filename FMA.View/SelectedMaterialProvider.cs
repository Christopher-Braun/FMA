using FMA.View.Models;

namespace FMA.View
{
    public class SelectedMaterialProvider : NotifyPropertyChangedBase
    {
        private MaterialModel materialModel;

        public MaterialModel MaterialModel
        {
            get { return materialModel; }
            set
            {
                this.materialModel = value;
                this.materialModel.PropertyChanged += (s, e) => OnPropertyChanged(e.PropertyName);
                OnPropertyChanged();
            }
        }
    }
}