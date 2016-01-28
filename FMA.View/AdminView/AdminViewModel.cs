using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using FMA.Contracts;
using FMA.View.Common;
using FMA.View.Helpers;
using FMA.View.Models;

namespace FMA.View.AdminView
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
        }

        private MaterialModel CreateEmptyMaterialTemplate(IFontService fontService)
        {
            var backgroundBytes = ImageHelper.ImageToByte(Properties.Resources.flyer_sample);
            return new MaterialModel(-1, "Leeres Material (*)", "", Enumerable.Empty<MaterialFieldModel>(), backgroundBytes, backgroundBytes, fontService.AllFontFamilies.First());
        }

        public void OpenTemplate(object sender, EventArgs args)
        {
            var id = ((FrameworkElement)sender).Tag;

            this.SelectedMaterial = Materials.First(m => m.Id.Equals(id));
            this.ViewState = ViewStates.SetCommonProperties;
        }

        public void SetFlyerFrontSideImage()
        {
            var fileName = LogoExtensions.ShowFileOpenDialog();
            if (string.IsNullOrEmpty(fileName))
            {
                return;
            }

            var byteArrayFromFile = LogoExtensions.GetByteArrayFromFile(fileName);
            this.SelectedMaterial.FlyerFrontSide = byteArrayFromFile;
        }

        public void SetFlyerBackSideImage()
        {
            var fileName = LogoExtensions.ShowFileOpenDialog();
            if (string.IsNullOrEmpty(fileName))
            {
                return;
            }

            this.SelectedMaterial.FlyerBackside = LogoExtensions.GetByteArrayFromFile(fileName);
        }

        public void ApplyCommonProperties()
        {
            this.ViewState = ViewStates.LayoutMode;
        }

        public void Create()
        {
            MaterialCreated(this.SelectedMaterial.ToNewMaterial());
        }

        public void CompleteReset()
        {
            this.SelectedMaterial = null;
            this.ViewState = ViewStates.SelectTemplate;
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
