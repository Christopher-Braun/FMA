// Christopher Braun 2016

using System;
using FMA.Contracts;
using FMA.View.Models;

namespace FMA.View.Common
{
    public abstract class FlyerViewModelBase : NotifyPropertyChangedBase
    {
        private ViewStates viewState;
        private bool canAddLogo;

        protected FlyerViewModelBase(SelectedMaterialProvider selectedMaterialProvider, IFontService fontService,
            ViewStates viewState)
        {
            this.viewState = viewState;
            SelectedMaterialProvider = selectedMaterialProvider;
            FontService = fontService;
            CanAddLogo = true;
        }

        public SelectedMaterialProvider SelectedMaterialProvider { get; }

        public IFontService FontService { get; }

        public abstract bool CanCreate { get; }

        public bool CanAddLogo
        {
            get { return canAddLogo; }
            set
            {
                canAddLogo = value;
                OnPropertyChanged();
            }
        }

        public void AddLogo()
        {
            if (CanAddLogo == false)
            {
                throw new InvalidOperationException(nameof(AddLogo));
            }

            SelectedMaterialProvider.MaterialModel.AddLogo();
        }

        public void DeleteLogo()
        {
            if (CanAddLogo == false)
            {
                throw new InvalidOperationException(nameof(DeleteLogo));
            }

            SelectedMaterialProvider.MaterialModel.LogoModel.DeleteLogo();
        }

        public void ToggleViews()
        {
            switch (ViewState)
            {
                case ViewStates.Both:
                    ViewState = ViewStates.OnlyInput;
                    break;
                case ViewStates.OnlyInput:
                    ViewState = ViewStates.OnlyPreview;
                    break;
                case ViewStates.OnlyPreview:
                    ViewState = ViewStates.Both;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
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
            Both,
            OnlyInput,
            OnlyPreview
        }
    }
}