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
using FMA.View.Properties;

namespace FMA.View.Common
{
    public abstract class FlyerMakerViewModelBase : INotifyPropertyChanged
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

        protected FlyerMakerViewModelBase(List<Material> materials, Func<string, FontInfo> getFont, IFontService fontService, IWindowService windowService)
        {
            this.fontService = fontService;
            FontHelper.UpdateFonts(materials, getFont, this.fontService);

            this.windowService = windowService;

            this.SelectedMaterialProvider = new SelectedMaterialProvider();
            this.SelectedMaterialProvider.PropertyChanged += (s, e) => OnPropertyChanged("CanCreate");

        }

        public SelectedMaterialProvider SelectedMaterialProvider { get; private set; }

        public bool ExternalPreviewVisible
        {
            get { return externalPreviewVisible; }
            set
            {
                externalPreviewVisible = value;

                if (value)
                {
                    windowService.OpenExternalPreviewWindow(this.SelectedMaterialProvider, this.fontService);
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
                    windowService.OpenExternalEditWindow(this.SelectedMaterialProvider, this.fontService);
                }
                else
                {
                    windowService.CloseExternalEditWindow();
                }
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
                    this.FlyerViewModel = new LayoutViewModel(this.SelectedMaterialProvider, fontService, viewState);
                }
                else
                {
                    this.FlyerViewModel = new DefaultViewModel(this.SelectedMaterialProvider, fontService, viewState);
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
                this.SelectedMaterialProvider.MaterialModel = value.Clone();
                OnPropertyChanged();
            }
        }

        public bool CanCreate
        {
            get
            {
                if (this.FlyerViewModel == null)
                {
                    return false;
                }
                return this.FlyerViewModel.CanCreate;
            }
        }

        public void Reset()
        {
            this.SelectedMaterial = this.Materials.Single(m => m.Id.Equals(this.SelectedMaterial.Id));
        }

        public virtual event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}