using System;
using System.ComponentModel;
using System.Windows.Media;
using FMA.Core;
using FMA.View.Models;

namespace FMA.View
{
    public class SelectedMaterialProvider : NotifyPropertyChangedBase
    {
        private MaterialModel selectedMaterial;

        public MaterialModel SelectedMaterial
        {
            get { return selectedMaterial; }
            set
            {
                if (selectedMaterial != null)
                {
                    this.selectedMaterial.PropertyChanged -= selectedMaterial_PropertyChanged;
                }
                if (value == null)
                {
                    this.selectedMaterial = null;
                    return;
                }

                this.selectedMaterial = value;

                //TODO Warum geht das nicht beim ErrorState
                this.selectedMaterial.PropertyChanged += selectedMaterial_PropertyChanged;
                selectedMaterial_PropertyChanged(null,null);
                OnPropertyChanged();
            }
        }

        private void selectedMaterial_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            MaterialChanged();
            flyerPreview = null;
            base.OnPropertyChanged("FlyerPreview");
        }

        public event Action MaterialChanged = () => { };

        private ImageSource flyerPreview;
        public ImageSource FlyerPreview
        {
            get
            {
                if (SelectedMaterial == null)
                {
                    return null;
                }

                if (flyerPreview == null)
                {
                    this.flyerPreview = FlyerCreator.CreateImage(SelectedMaterial.ToCustomMaterial());
                }

                return flyerPreview;
            }
        }
    }
}