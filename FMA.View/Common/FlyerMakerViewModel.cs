using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FMA.Contracts;
using FMA.View.DefaultView;
using FMA.View.Helpers;
using FMA.View.LayoutView;
using FMA.View.Models;
using FMA.View.Properties;

namespace FMA.View.Common
{
    public class FlyerMakerViewModel : INotifyPropertyChanged
    {
        public event Action<CustomMaterial> FlyerCreated = m => { };

        private List<MaterialModel> materials;
        private bool externalPreviewVisible;
        private bool externalEditVisible;
        private bool layoutMode;
        private bool layoutButtonsVisible;
        private FlyerViewModelBase flyerViewModel;

        private readonly SelectedMaterialProvider selectedMaterialProvider;
        private readonly IWindowService windowService;
        private readonly IFontService fontService;

        public FlyerMakerViewModel(List<Material> materials, int selectedMateriaId, Func<string, FontInfo> getFont,IFontService fontService , IWindowService windowService)
        {
            this.fontService = fontService;
            UpdateFonts(materials, getFont);

            this.windowService = windowService;

            this.selectedMaterialProvider = new SelectedMaterialProvider();
            this.selectedMaterialProvider.PropertyChanged += (s, e) => OnPropertyChanged("CanCreate");

            this.SetMaterials(materials, selectedMateriaId);

            this.LayoutMode = true;
        }

        private void UpdateFonts(IEnumerable<Material> materials, Func<string, FontInfo> getFont)
        {
            var requiredFonts = materials.SelectMany(m => m.MaterialFields).Select(f => f.FontName);

            foreach (var requiredFontName in requiredFonts)
            {
                if (fontService.IsFontAvailable(requiredFontName))
                {
                    continue;
                }
                var fontInfo = getFont(requiredFontName);
                if (fontInfo != null)
                {
                    fontService.InstallFont(fontInfo.FileName, fontInfo.Buffer);
                }
            }
        }

        private void SetMaterials(IEnumerable<Material> materials, int selectedMateriaId = -1)
        {
            this.Materials = materials.Select(m => m.ToMaterialModel(fontService)).ToList();
            if (selectedMateriaId != -1)
            {
                this.SelectedMaterial = this.Materials.Single(m => m.Id.Equals(selectedMateriaId));
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
                    windowService.OpenExternalPreviewWindow(this.selectedMaterialProvider, this.fontService);
                }
                else
                {
                    windowService.CloseExternalPreviewWindow();
                }
            }
        }

        public bool ExternalEditVisible
        {
            get { return externalEditVisible; }
            set
            {
                externalEditVisible = value;

                if (value)
                {
                    windowService.OpenExternalEditWindow(this.selectedMaterialProvider, this.fontService);
                }
                else
                {
                    windowService.CloseExternalEditWindow();
                }
            }
        }

        public bool LayoutButtonsVisible
        {
            get { return layoutButtonsVisible; }
            set
            {
                layoutButtonsVisible = value;
                OnPropertyChanged();
            }
        }

        public void EnableLayoutButtons()
        {
            if (Keyboard.Modifiers == (ModifierKeys.Shift | ModifierKeys.Control))
            {
                LayoutButtonsVisible = !LayoutButtonsVisible;
            }
        }

        public bool LayoutMode
        {
            get { return layoutMode; }
            set
            {
                layoutMode = value;

                var viewState = FlyerViewModelBase.ViewStates.Both;

                if (FlyerViewModel != null)
                {
                    viewState = FlyerViewModel.ViewState;

                }

                if (layoutMode)
                {
                    this.FlyerViewModel = new LayoutViewModel(this.selectedMaterialProvider, fontService, viewState);
                }
                else
                {
                    this.FlyerViewModel = new DefaultViewModel(this.selectedMaterialProvider, fontService, viewState);
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
            get { return selectedMaterialProvider.MaterialModel; }
            set { this.selectedMaterialProvider.MaterialModel = value.Clone(); }
        }

        public bool CanCreate
        {
            get { return this.FlyerViewModel.CanCreate; }
        }

        public void Create()
        {
            FlyerCreated(SelectedMaterial.ToCustomMaterial());
        }

        public void Reset()
        {
            this.SelectedMaterial = this.Materials.Single(m => m.Id.Equals(this.SelectedMaterial.Id));
        }

        //public void AddLogo()
        //{
        //    this.selectedMaterialProvider.MaterialModel.AddLogo();
        //}

        //public void DeleteLogo()
        //{
        //    this.selectedMaterialProvider.MaterialModel.LogoModel.DeleteLogo();
        //}

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
