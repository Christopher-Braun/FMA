using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using FMA.Contracts;
using FMA.Core;
using FMA.View.Properties;

namespace FMA.View
{
    public class FlyerMakerViewModel : INotifyPropertyChanged, IFlyerMakerView
    {
        public event Action<CustomMaterial> FlyerCreated = m => { };

        private bool bothVisible;
        private List<MaterialModel> materials;
        private readonly ExternalPreviewView externalPreviewView;
        private bool previewVisible;
        private bool externalPreviewVisible;
        private bool inputVisible;

        public FlyerMakerViewModel()
        {
            this.BothVisible = true;
            externalPreviewView = new ExternalPreviewView(() => this.FlyerPreview);
            this.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "FlyerPreview")
                {
                    externalPreviewView.FlyerChanged();
                }
            };
        }

        public bool ExternalPreviewVisible
        {
            get { return externalPreviewVisible; }
            set
            {
                externalPreviewVisible = value;

                if (value)
                {
                    externalPreviewView.Show();
                }
                else
                {
                    externalPreviewView.Hide();
                }

                OnPropertyChanged();
            }
        }

        public bool LayoutMode
        {
            get { return layoutMode; }
            set
            {
                layoutMode = value;
                OnPropertyChanged();
            }
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
        private bool layoutMode;

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

                this.selectedMaterial.PropertyChanged += selectedMaterial_PropertyChanged;
                selectedMaterial_PropertyChanged(null, null);
            }
        }

        private void selectedMaterial_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("CanCreate");
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
