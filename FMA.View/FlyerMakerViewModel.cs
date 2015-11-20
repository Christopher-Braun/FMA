using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using FMA.Contracts;
using FMA.View.Properties;

namespace FMA.View
{
    public class FlyerMakerViewModel : INotifyPropertyChanged, IFlyerMakerView
    {
        public event Action<CustomMaterial> FlyerCreated = m => { };

        private bool bothVisible;
        private List<MaterialModel> materials;
        private ExternalPreviewView externalPreviewView;
        private bool previewVisible;
        private bool externalPreviewVisible;
        private bool inputVisible;

        private readonly SelectedMaterialProvider selectedMaterialProvider;

        public FlyerMakerViewModel()
        {
            this.selectedMaterialProvider = new SelectedMaterialProvider();
            this.selectedMaterialProvider.MaterialChanged += () => { OnPropertyChanged("CanCreate"); };

            this.LayoutMode = false;
            this.BothVisible = true;
        }

        public FlyerViewModelBase SubView
        {
            get { return subView; }
            private set
            {
                subView = value;
                OnPropertyChanged();
            }
        }

        public bool ExternalPreviewVisible
        {
            get { return externalPreviewVisible; }
            set
            {
                externalPreviewVisible = value;

                if (value)
                {
                    externalPreviewView = new ExternalPreviewView(this.selectedMaterialProvider);
                    externalPreviewView.Show();
                }
                else
                {
                    externalPreviewView.Close();
                    externalPreviewView = null;
                }
            }
        }

        public bool LayoutMode
        {
            get { return layoutMode; }
            set
            {
                layoutMode = value;

                if (layoutMode)
                {
                    this.SubView = new LayoutViewModel(this.selectedMaterialProvider, previewVisible, inputVisible,bothVisible);
                }
                else
                {
                    SubView = new DefaultViewModel(this.selectedMaterialProvider, previewVisible, inputVisible, bothVisible);
                }

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

        private bool layoutMode;
        private FlyerViewModelBase subView;

        public MaterialModel SelectedMaterial
        {
            get { return selectedMaterialProvider.SelectedMaterial; }
            set
            {
                this.selectedMaterialProvider.SelectedMaterial = value;
                OnPropertyChanged();
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

        public bool CanCreate
        {
            get { return this.SubView.CanCreate; }
        }


        public bool PreviewVisible
        {
            get { return previewVisible; }
            set
            {
                if (value.Equals(previewVisible)) return;
                previewVisible = value;
                this.SubView.PreviewVisible = value;
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
                this.SubView.InputVisible = value;
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
                this.SubView.BothVisible = value;
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
