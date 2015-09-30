using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using FMA.Contracts.Properties;

namespace FMA.View
{
    public class MaterialModel : INotifyPropertyChanged
    {
        private readonly List<MaterialFieldModel> materialFields;

        public MaterialModel(int id, string title, string description, IEnumerable<MaterialFieldModel> materialFields)
        {
            Id = id;
            Title = title;
            Description = description;

            this.materialFields = new List<MaterialFieldModel>();
            materialFields.ToList().ForEach(m =>
            {
                this.materialFields.Add(m);
                m.PropertyChanged += (s, e) => OnPropertyChanged(e.PropertyName);
            });
        }

        public Int32 Id { get; private set; }

        public string Title { get; private set; }

        public string Description { get; private set; }

        public IEnumerable<MaterialFieldModel> MaterialFields
        {
            get
            {
                return materialFields;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
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
            return Equals((MaterialModel) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
