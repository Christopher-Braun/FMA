// Christopher Braun 2016

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media.Imaging;
using FMA.Contracts;
using FMA.View.Properties;

namespace FMA.View.Models
{
    public class MaterialModel : NotifyPropertyChangedBase
    {
        private byte[] flyerFrontSide;
        private byte[] flyerBackside;
        private BitmapImage flyerFrontSideImage;
        private BitmapImage flyerBackSideImage;

        public MaterialModel(int id, string title, string description, IEnumerable<MaterialFieldModel> materialFields,
            byte[] flyerFrontSide, byte[] flyerBackside, FontFamilyWithName defaultFont)
        {
            MaterialFields = new ObservableCollection<MaterialFieldModel>();

            Id = id;
            Title = title;
            Description = description;

            materialFields.ToList().ForEach(AddMaterialField);

            MaterialFields.CollectionChanged += (s, e) => OnPropertyChanged("MaterialFields");

            FlyerFrontSide = flyerFrontSide;
            FlyerBackside = flyerBackside;
            DefaultFont = defaultFont;

            LogoModel = new LogoModel();
            LogoModel.PropertyChanged += (s, e) => OnPropertyChanged("Logo");
        }

        public int Id { get; }

        public string Title { get; }

        public string Description { get;}

        public ObservableCollection<MaterialFieldModel> MaterialFields { get; }

        public LogoModel LogoModel { get; }

        public FontFamilyWithName DefaultFont { get; }

        public BitmapImage FlyerFrontSideImage
        {
            get { return flyerFrontSideImage; }
            private set
            {
                flyerFrontSideImage = value;
                OnPropertyChanged();
            }
        }

        public byte[] FlyerFrontSide
        {
            get { return flyerFrontSide; }
            set
            {
                flyerFrontSide = value;
                FlyerFrontSideImage = FlyerFrontSide.GetBitmapImage();
                OnPropertyChanged();
            }
        }

        public BitmapImage FlyerBackSideImage
        {
            get { return flyerBackSideImage; }
            set
            {
                flyerBackSideImage = value;
                OnPropertyChanged();
            }
        }

        public byte[] FlyerBackside
        {
            get { return flyerBackside; }
            set
            {
                flyerBackside = value;
                FlyerBackSideImage = FlyerBackside.GetBitmapImage();
                OnPropertyChanged();
            }
        }

        public void AddNewMaterialField()
        {
            var materialField = new MaterialFieldModel("CustomField", Resources.CustomFieldText, DefaultFont);
            AddMaterialField(materialField);
        }

        private void AddMaterialField(MaterialFieldModel materialField)
        {
            materialField.PropertyChanged += (s, e) => OnPropertyChanged("MaterialField");
            MaterialFields.Add(materialField);
        }

        protected bool Equals(MaterialModel other) => Id == other.Id;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((MaterialModel) obj);
        }

        public override int GetHashCode() => Id;
    }
}