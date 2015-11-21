using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using FMA.Contracts;
using FMA.View.Models;
using FMA.View.Properties;

namespace FMA.View
{
    public class FlyerMakerViewModel : INotifyPropertyChanged, IFlyerMakerView
    {
        public event Action<CustomMaterial> FlyerCreated = m => { };
        public WindowService WindowService { get; set; }
       
        private List<MaterialModel> materials;
        private bool externalPreviewVisible;
        private bool layoutMode;
        private FlyerViewModelBase flyerViewModel;
        private readonly SelectedMaterialProvider selectedMaterialProvider;

        public FlyerMakerViewModel(IEnumerable<Material> materials, int selectedMateriaId = -1)
        {
            this.selectedMaterialProvider = new SelectedMaterialProvider();
            this.selectedMaterialProvider.MaterialChanged += () => { OnPropertyChanged("CanCreate"); };

            this.SetMaterials(materials, selectedMateriaId);

            this.LayoutMode = false;
            this.BothVisible = true;
        }

        private void SetMaterials(IEnumerable<Material> materials, int selectedMateriaId = -1)
        {
            this.Materials = materials.Select(m => m.ToMaterialModel()).ToList();
            if (selectedMateriaId != -1)
            {
                this.SelectedMaterial = this.Materials.Single(m=> m.Id.Equals(selectedMateriaId));
            }
        }

        public FlyerViewModelBase FlyerViewModel
        {
            get { return flyerViewModel; }
            private set
            {
                flyerViewModel = value;
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
                    WindowService.OpenExternalPreviewWindow(this.selectedMaterialProvider);
                }
                else
                {
                    WindowService.CloseExternalPreviewWindow();
                }
            }
        }

        public bool LayoutMode
        {
            get { return layoutMode; }
            set
            {
                layoutMode = value;

                bool previewVisible= false;
                bool inputVisible= false;
                bool bothVisible = true;

                if (flyerViewModel != null)
                {
                    previewVisible = flyerViewModel.PreviewVisible;
                    inputVisible = flyerViewModel.InputVisible;
                    bothVisible = flyerViewModel.BothVisible;
                }

                if (layoutMode)
                {
                    this.FlyerViewModel = new LayoutViewModel(this.selectedMaterialProvider, previewVisible,inputVisible, bothVisible);
                }
                else
                {
                    this.FlyerViewModel = new DefaultViewModel(this.selectedMaterialProvider, previewVisible,inputVisible, bothVisible);
                }

                OnPropertyChanged("CanCreate");
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

        public MaterialModel SelectedMaterial
        {
            get { return selectedMaterialProvider.SelectedMaterial; }
            set{this.selectedMaterialProvider.SelectedMaterial = value.Clone();}
        }

        public void Create()
        {
            FlyerCreated(SelectedMaterial.ToCustomMaterial());
        }

        public void Reset()
        {
            this.SelectedMaterial = this.Materials.Single(m => m.Id.Equals(this.SelectedMaterial.Id));
        }

        public bool CanCreate
        {
            get { return this.FlyerViewModel.CanCreate; }
        }

        public bool PreviewVisible
        {
            get { return this.FlyerViewModel.PreviewVisible; }
            set{this.FlyerViewModel.PreviewVisible = value;}}

        public bool InputVisible
        {
            get { return this.FlyerViewModel.InputVisible; }
            set{this.FlyerViewModel.InputVisible = value;}
        }

        public bool BothVisible
        {
            get { return this.FlyerViewModel.BothVisible; }
            set{this.FlyerViewModel.BothVisible = value;}
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
