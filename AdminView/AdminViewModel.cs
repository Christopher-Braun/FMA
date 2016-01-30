using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using FMA.Contracts;
using FMA.View.Common;
using FMA.View.Helpers;
using FMA.View.Models;
using FMA.View.Properties;

namespace FMA.AdminView
{
    public class AdminViewModel : FlyerMakerViewModelBase
    {
        private ViewStates viewState;
        public event Action<Material> MaterialCreated = m => { };

        public AdminViewModel(List<Material> materials, Func<string, FontInfo> getFont, IFontService fontService, IWindowService windowService)
            : base(materials, getFont, fontService, windowService)
        {
            this.Materials = materials.Select(m => m.ToMaterialModel(fontService)).ToList();
            this.Materials.Insert(0, CreateEmptyMaterialTemplate(fontService));

            this.ViewState = ViewStates.SelectTemplate;
            this.LayoutMode = true;
            this.FlyerViewModel.CanAddLogo = false;
        }

        private static MaterialModel CreateEmptyMaterialTemplate(IFontService fontService)
        {
            var backgroundBytes = Properties.Resources.flyer_sample.ToByteArray();
            return new MaterialModel(-1, Resources.EmptyMaterial, "", Enumerable.Empty<MaterialFieldModel>(), backgroundBytes, backgroundBytes, fontService.AllFontFamilies.First());
        }

        public void OpenTemplate(object sender, EventArgs args)
        {
            var id = ((FrameworkElement)sender).Tag;

            this.SelectedMaterial = Materials.First(m => m.Id.Equals(id));
            this.ViewState = ViewStates.SetCommonProperties;
        }

        public void SetFlyerFrontSideImage()
        {
            SetFlyerImage(b => this.SelectedMaterial.FlyerFrontSide = b);
        }

        public void SetFlyerBackSideImage()
        {
            SetFlyerImage(b => this.SelectedMaterial.FlyerBackside = b);
        }

        private static void SetFlyerImage(Action<byte[]> setImage)
        {
            var fileName = FileHelper.ShowFileOpenDialogForImages();
            if (string.IsNullOrEmpty(fileName))
            {
                return;
            }

            var bytes = FileHelper.GetByteArrayFromFile(fileName);
            setImage(bytes);
        }

        public void ApplyCommonProperties()
        {
            this.ViewState = ViewStates.LayoutMode;
        }

        public void Create()
        {
            MaterialCreated(this.SelectedMaterial.ToNewMaterial());
        }

        public void Back()
        {
            if (ViewState == ViewStates.LayoutMode)
            {
                this.ViewState = ViewStates.SetCommonProperties;
            }
            else if (ViewState == ViewStates.SetCommonProperties)
            {
                this.SelectedMaterial = null;
                this.ViewState = ViewStates.SelectTemplate;
            }
        }

        public ViewStates ViewState
        {
            get { return viewState; }
            set
            {
                viewState = value;
                OnPropertyChanged();
            }
        }

        public enum ViewStates
        {
            SelectTemplate,
            SetCommonProperties,
            LayoutMode,
        }


    }
}
