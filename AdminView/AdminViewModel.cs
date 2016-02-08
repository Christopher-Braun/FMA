// Christopher Braun 2016

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using FMA.AdminView.Properties;
using FMA.Contracts;
using FMA.View.Common;
using FMA.View.Helpers;
using FMA.View.Models;

namespace FMA.AdminView
{
    public class AdminViewModel : FlyerMakerViewModelBase
    {
        private ViewStates viewState;
        public event Action<Material> MaterialCreated = m => { };

        public AdminViewModel(List<Material> materials, Func<string, FontInfo> getFont, IFontService fontService,
            IWindowService windowService)
            : base(materials, getFont, fontService, windowService)
        {
            Materials = materials.Select(m => m.ToMaterialModel(fontService)).ToList();
            Materials.Insert(0, CreateEmptyMaterialTemplate(fontService));

            ViewState = ViewStates.SelectTemplate;
            LayoutMode = true;
            FlyerViewModel.CanAddLogo = false;
        }

        private static MaterialModel CreateEmptyMaterialTemplate(IFontService fontService)
        {
            var backgroundBytes = Resources.flyer_sample.ToByteArray();
            return new MaterialModel(-1, View.Properties.Resources.EmptyMaterial, "",
                Enumerable.Empty<MaterialFieldModel>(), backgroundBytes, backgroundBytes,
                fontService.AllFontFamilies.First());
        }

        public void OpenTemplate(object sender, EventArgs args)
        {
            var id = ((FrameworkElement) sender).Tag;

            SelectedMaterial = Materials.First(m => m.Id.Equals(id));
            ViewState = ViewStates.SetCommonProperties;
        }

        public void SetFlyerFrontSideImage()
        {
            SetFlyerImage(b => SelectedMaterial.FlyerFrontSide = b);
        }

        public void SetFlyerBackSideImage()
        {
            SetFlyerImage(b => SelectedMaterial.FlyerBackside = b);
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
            ViewState = ViewStates.LayoutMode;
        }

        public void Create()
        {
            MaterialCreated(SelectedMaterial.ToNewMaterial());
        }

        public void Back()
        {
            if (ViewState == ViewStates.LayoutMode)
            {
                ViewState = ViewStates.SetCommonProperties;
            }
            else if (ViewState == ViewStates.SetCommonProperties)
            {
                SelectedMaterial = null;
                ViewState = ViewStates.SelectTemplate;
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
            LayoutMode
        }
    }
}