// Christopher Braun 2016

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using FMA.Contracts;
using FMA.View.DefaultView;
using FMA.View.Helpers;
using FMA.View.LayoutView;
using FMA.View.Models;

namespace FMA.View.Common
{
    public abstract class FlyerMakerViewModelBase : NotifyPropertyChangedBase
    {
        private List<MaterialModel> materials;
        private bool externalPreviewVisible;
        private bool externalEditVisible;
        private bool layoutMode;
        private readonly IWindowService windowService;
        private readonly IFontService fontService;
        private FlyerViewModelBase flyerViewModel;

        public FlyerViewModelBase FlyerViewModel
        {
            get { return flyerViewModel; }
            private set
            {
                flyerViewModel = value;
                OnPropertyChanged();
            }
        }

        protected FlyerMakerViewModelBase(List<Material> materials, Func<string, FontInfo> getFont,
            IFontService fontService, IWindowService windowService)
        {
            this.fontService = fontService;
            FontHelper.UpdateFonts(materials, getFont, this.fontService);

            this.windowService = windowService;

            SelectedMaterialProvider = new SelectedMaterialProvider();
            SelectedMaterialProvider.PropertyChanged += (s, e) => OnPropertyChanged("CanCreate");
        }

        public SelectedMaterialProvider SelectedMaterialProvider { get; }

        public bool ExternalPreviewVisible
        {
            get { return externalPreviewVisible; }
            set
            {
                externalPreviewVisible = value;

                if (value)
                {
                    windowService.OpenExternalPreviewWindow(SelectedMaterialProvider, fontService);
                }
                else
                {
                    windowService.CloseExternalPreviewWindow();
                }

                OnPropertyChanged();
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
                    windowService.OpenExternalEditWindow(SelectedMaterialProvider, fontService);
                }
                else
                {
                    windowService.CloseExternalEditWindow();
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

                var viewState = FlyerViewModelBase.ViewStates.Both;

                if (FlyerViewModel != null)
                {
                    viewState = FlyerViewModel.ViewState;
                }

                if (layoutMode)
                {
                    FlyerViewModel = new LayoutViewModel(SelectedMaterialProvider, fontService, viewState);
                }
                else
                {
                    FlyerViewModel = new DefaultViewModel(SelectedMaterialProvider, fontService, viewState);
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
            get { return SelectedMaterialProvider.MaterialModel; }
            set
            {
                SelectedMaterialProvider.MaterialModel = value.Clone();
                ExternalEditVisible = false;
                ExternalPreviewVisible = false;
                OnPropertyChanged();
            }
        }

        public bool CanCreate => FlyerViewModel != null && FlyerViewModel.CanCreate;

        public void Reset()
        {
            SelectedMaterial = Materials.Single(m => m.Id.Equals(SelectedMaterial.Id));
        }
    }
}