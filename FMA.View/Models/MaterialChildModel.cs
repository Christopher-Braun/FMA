// Christopher Braun 2016

namespace FMA.View.Models
{
    public class MaterialChildModel : NotifyPropertyChangedBase
    {
        private string fieldName;
        private bool isSelected;

        public string FieldName
        {
            get { return fieldName; }
            set
            {
                fieldName = value;
                OnPropertyChanged();
            }
        }

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (isSelected == value)
                {
                    return;
                }
                isSelected = value;
                OnPropertyChanged();
            }
        }
    }
}