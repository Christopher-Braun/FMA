using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using FMA.Contracts;
using FMA.View.Helpers;
using FMA.View.Models;
using FMA.View.Properties;
using Microsoft.Win32;

namespace FMA.View
{
    public class FlyerMakerViewModel : INotifyPropertyChanged
    {
        public event Action<CustomMaterial> FlyerCreated = m => { };

        public WindowService WindowService { get; set; }

        private List<MaterialModel> materials;
        private bool externalPreviewVisible;
        private bool externalEditVisible;
        private bool layoutMode;
        private FlyerViewModelBase flyerViewModel;
        private readonly SelectedMaterialProvider selectedMaterialProvider;
        private bool layoutButtonsVisible;
        private readonly FontService fontService;

        public FlyerMakerViewModel(List<Material> materials, int selectedMateriaId, Func<string, FontInfo> getFont, string customFontsDir)
        {
            fontService = new FontService(customFontsDir);

            UpdateFonts(materials, getFont);

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
                    WindowService.OpenExternalPreviewWindow(this.selectedMaterialProvider, this.fontService);
                }
                else
                {
                    WindowService.CloseExternalPreviewWindow();
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
                    WindowService.OpenExternalEditWindow(this.selectedMaterialProvider, this.fontService);
                }
                else
                {
                    WindowService.CloseExternalEditWindow();
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

        public void AddLogo()
        {
            var dialog = new OpenFileDialog
            {
                //TODO Remove when going productive
                Title = "Select logo file",
                InitialDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
                Filter = "Image files | *.jpg; *.jpeg; *.bmp; *.png; *.gif | All Files | *.*"
            };

            var result = dialog.ShowDialog();
            if (result != true)
            {
                return;
            }

            this.selectedMaterialProvider.MaterialModel.SetLogo(dialog.FileName, new Point(25, 75));
        }

        public void DeleteLogo()
        {
            this.selectedMaterialProvider.MaterialModel.LogoModel.DeleteLogo();
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
