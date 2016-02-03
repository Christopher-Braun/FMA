// Christopher Braun 2016

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using FMA.View.Models;

namespace FMA.View.Common
{
    public class SelectedMaterialProvider : NotifyPropertyChangedBase
    {
        private MaterialModel materialModel;
        private bool showBackSide;

        public SelectedMaterialProvider()
        {
            MaterialChilds = new ObservableCollection<MaterialChildModel>();
        }

        public ObservableCollection<MaterialChildModel> MaterialChilds { get; }

        public MaterialModel MaterialModel
        {
            get { return materialModel; }
            set
            {
                materialModel = value;
                OnPropertyChanged();
                MaterialChilds.Clear();

                if (materialModel == null) return;
                materialModel.PropertyChanged += (s, e) => OnPropertyChanged(e.PropertyName);
                InitLogo(materialModel.LogoModel);

                var materialFields = materialModel.MaterialFields;
                InitMaterialFields(materialFields);

                if (MaterialChilds.Any())
                {
                    SetSelectedChilds(MaterialChilds.First());
                }
            }
        }

        private void InitMaterialFields(ObservableCollection<MaterialFieldModel> materialFields)
        {
            foreach (var materialField in materialFields)
            {
                AddChildToMaterialChilds(materialField);
            }
            materialFields.CollectionChanged += (s, e) =>
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                    {
                        foreach (var newItem in e.NewItems.OfType<MaterialChildModel>())
                        {
                            //Vor das Logo
                            AddChildToMaterialChilds(newItem);
                        }
                        break;
                    }
                    case NotifyCollectionChangedAction.Remove:
                    {
                        foreach (var oldItem in e.OldItems.OfType<MaterialChildModel>())
                        {
                            RemoveChildFromMaterialChilds(oldItem);
                        }
                        break;
                    }
                }
            };
        }

        private void InitLogo(LogoModel logoModel)
        {
            if (logoModel.HasLogo)
            {
                AddChildToMaterialChilds(logoModel);
            }
            logoModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "HasLogo")
                {
                    if (MaterialModel.LogoModel.HasLogo && MaterialChilds.Contains(logoModel) == false)
                    {
                        AddChildToMaterialChilds(logoModel);
                    }
                    if (MaterialModel.LogoModel.HasLogo == false && MaterialChilds.Contains(logoModel))
                    {
                        RemoveChildFromMaterialChilds(logoModel);
                    }
                }
            };
        }

        private void AddChildToMaterialChilds(MaterialChildModel materialChild)
        {
            materialChild.PropertyChanged += materialChild_PropertyChanged;
            MaterialChilds.Insert(MaterialChilds.Count - MaterialChilds.OfType<LogoModel>().Count(), materialChild);
        }

        private void RemoveChildFromMaterialChilds(MaterialChildModel materialChild)
        {
            materialChild.PropertyChanged -= materialChild_PropertyChanged;
            MaterialChilds.Remove(materialChild);
        }

        private void materialChild_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MaterialChildModel.IsSelected))
            {
                OnPropertyChanged(nameof(SelectedMaterialChilds));
            }
        }

        public void SetSelectedChilds(params MaterialChildModel[] childrenToSelect)
        {
            foreach (var materialChild in MaterialChilds)
            {
                materialChild.IsSelected = childrenToSelect.Contains(materialChild);
            }
        }

        public List<MaterialChildModel> SelectedMaterialChilds => MaterialChilds.Where(s => s.IsSelected).ToList();

        public bool ShowBackSide
        {
            get { return showBackSide; }
            set
            {
                showBackSide = value;
                OnPropertyChanged();
            }
        }
    }
}