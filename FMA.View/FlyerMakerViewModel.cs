using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using FMA.Contracts;
using FMA.Core;
using FMA.TestData;
using FMA.View.Properties;

namespace FMA.View
{
    public class FlyerMakerViewModel : INotifyPropertyChanged, IFlyerMakerView
    {

        public event Action<CustomMaterial> FlyerCreated = m => { };

        public FlyerMakerViewModel()
        {
            // TODO: Remove when going productive
            var dummyMaterials = DummyData.GetDummyMaterials();
            SetMaterials(dummyMaterials, dummyMaterials.First());

            this.BothVisible = true;
            MaterialFieldModel.EditLayoutMode = f=> FieldToEditLayout = f; 
        }

        public MaterialFieldModel FieldToEditLayout
        {
            get { return fieldToEditLayout; }
            set
            {
                fieldToEditLayout = value;
                OnPropertyChanged();
            }
        }

        public void ExitLayoutMode()
        {
            this.FieldToEditLayout = null;
        }


        public void SetMaterials(IEnumerable<Material> materials, Material selectedMaterial = null)
        {         
            this.Materials = materials.Select(m => m.ToMaterialModel()).ToList();
            if (selectedMaterial != null)
            {
                this.SelectedMaterial = selectedMaterial.ToMaterialModel();
            }
        }


        public List<MaterialModel> Materials
        {
            get { return materials; }
            set
            {
                materials = value; 
                OnPropertyChanged();
            }
        }

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

                this.selectedMaterial = value.Clone();
                
                OnPropertyChanged();
                UpdateFlyer();
                this.selectedMaterial.PropertyChanged += selectedMaterial_PropertyChanged;
            }
        }

        private void selectedMaterial_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateFlyer();
            OnPropertyChanged("CanCreate");
        }

        private void UpdateFlyer()
        {
            OnPropertyChanged("FlyerPreview");
        }

        public bool CanCreate
        {
            get
            {
                if (this.SelectedMaterial == null)
                {
                    return false;
                }

                return SelectedMaterial.MaterialFields.All(f => String.IsNullOrEmpty(f.Error));
            }
        }


        public ImageSource FlyerPreview
        {
            get
            {
                if (SelectedMaterial == null)
                {
                    return null;
                }
                return FlyerCreator.CreateImage(SelectedMaterial.ToCustomMaterial());
            }
        }


        public void Save()
        {
            FlyerCreated(SelectedMaterial.ToCustomMaterial());
        }

        public void Reset()
        {
            this.SelectedMaterial = this.Materials.Single(m => m.Id.Equals(this.SelectedMaterial.Id));
        }

        private bool previewVisible;
        public bool PreviewVisible
        {
            get { return previewVisible; }
            set
            {
                if (value.Equals(previewVisible)) return;
                previewVisible = value;
                OnPropertyChanged();
            }
        }

        private bool inputVisible;
        public bool InputVisible
        {
            get { return inputVisible; }
            set
            {
                if (value.Equals(inputVisible)) return;
                inputVisible = value;
                OnPropertyChanged();
            }
        }

        private bool bothVisible;
        private List<MaterialModel> materials;
        private MaterialFieldModel fieldToEditLayout;

        public bool BothVisible
        {
            get { return bothVisible; } 
            set
            {
                if (value.Equals(bothVisible)) return;
                bothVisible = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
