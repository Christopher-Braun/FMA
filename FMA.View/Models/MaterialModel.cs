using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using FMA.Contracts;
using FMA.View.Properties;

namespace FMA.View.Models
{
    public class MaterialModel : INotifyPropertyChanged
    {
        private byte[] flyerFrontSide;

        public MaterialModel(int id, string title, string description, IEnumerable<MaterialFieldModel> materialFields, byte[] flyerFrontSide, byte[] flyerBackside, FontFamilyWithName defaultFont)
        {
            MaterialFields = new ObservableCollection<MaterialFieldModel>();
            PropertyChanged += (s, e) => { };

            Id = id;
            Title = title;
            Description = description;

            materialFields.ToList().ForEach(AddMaterialField);

            MaterialFields.CollectionChanged += (s, e) => OnPropertyChanged("MaterialFields");

            FlyerFrontSide = flyerFrontSide;
            FlyerBackside = flyerBackside;
            DefaultFont = defaultFont;

            LogoModel = new LogoModel();
            LogoModel.PropertyChanged += (s, e) => PropertyChanged(s, e);
        }

        public Int32 Id { get; private set; }

        public string Title { get; private set; }

        public string Description { get; private set; }

        public ObservableCollection<MaterialFieldModel> MaterialFields
        {
            get;
            private set;
        }

        public void AddNewMaterialField()
        {
            var materialField = new MaterialFieldModel("CustomField", Resources.CustomFieldText, DefaultFont);

            AddMaterialField(materialField);
        }

        private void AddMaterialField(MaterialFieldModel materialField)
        {
            materialField.PropertyChanged += (s, e) => PropertyChanged(s, e);
            this.MaterialFields.Add(materialField);
        }

        public BitmapImage FlyerFrontSideImage { get; private set; }

        public Byte[] FlyerFrontSide
        {
            get { return flyerFrontSide; }
            private set
            {
                flyerFrontSide = value;
                FlyerFrontSideImage = FlyerFrontSide.GetBitmapImage();
            }
        }

        public Byte[] FlyerBackside { get; private set; }
        public FontFamilyWithName DefaultFont { get; set; }

        public LogoModel LogoModel { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [Contracts.Properties.NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool Equals(MaterialModel other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MaterialModel)obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
