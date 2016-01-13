using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FMA.Contracts;
using FMA.View.Models;
using FMA.View.Properties;

namespace FMA.View.Common
{
    public abstract class FlyerViewModelBase : INotifyPropertyChanged
    {
        private ViewStates viewState;

        protected FlyerViewModelBase(SelectedMaterialProvider selectedMaterialProvider, FontService fontService, ViewStates viewState)
        {
            this.viewState = viewState;
            SelectedMaterialProvider = selectedMaterialProvider;
            FontService = fontService;
        }

        public SelectedMaterialProvider SelectedMaterialProvider { get; private set; }

        public FontService FontService { get; private set; }

        public abstract bool CanCreate { get; }

        public void AddLogo()
        {
            SelectedMaterialProvider.MaterialModel.AddLogo();
            //TODO Only in override
      //      SelectedMaterialProvider.SetSelectedChilds(SelectedMaterialProvider.MaterialModel.LogoModel);
        }

        public void DeleteLogo()
        {
            SelectedMaterialProvider.MaterialModel.LogoModel.DeleteLogo();
         //   SelectedMaterialProvider.SetSelectedChilds(SelectedMaterialProvider.MaterialModel.MaterialFields.Last());
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


        public virtual event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public enum ViewStates
        {
            Both,
            OnlyInput,
            OnlyPreview,
        }
    }
}