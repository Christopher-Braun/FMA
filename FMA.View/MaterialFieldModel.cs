using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using FMA.Contracts.Properties;

namespace FMA.View
{
    [DebuggerDisplay("Name: {FieldName} Valu:e {Value}")]
    public class MaterialFieldModel : INotifyPropertyChanged, IDataErrorInfo
    {
        public MaterialFieldModel(string fieldName, string fontName, int fontSize, bool bold, bool italic, bool uppper, int maxLength, int maxRows, int leftMargin, int topMargin, string value)
        {
            FieldName = fieldName;
            FontName = fontName;
            FontSize = fontSize;
            Bold = bold;
            Italic = italic;
            Uppper = uppper;
            MaxLength = maxLength;
            MaxRows = maxRows;
            LeftMargin = leftMargin;
            TopMargin = topMargin;
            Value = value;
        }

        public string FieldName { get; set; }
        public string FontName { get; set; }
        public int FontSize { get; set; }
        public bool Bold { get; set; }
        public bool Italic { get; set; }
        public bool Uppper { get; set; }
        public int MaxLength { get; set; }
        public int MaxRows { get; set; }
        public int LeftMargin { get; set; }
        public int TopMargin { get; set; }

        private string value;
        public string Value
        {
            get { return value; }
            set
            {
                if (value == this.value) return;
                this.value = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public string this[string columnName]
        {
            get
            {
                var errorText = string.Empty;

                if (columnName == "Value")
                {
                    var newLine = new[] {"\r\n"};
                    var lines = Value.Split(newLine, StringSplitOptions.None);

                    if (lines.Aggregate((s1, s2) => s1 + s2).Length > MaxLength)
                    {
                        errorText = String.Format("Der Text darf maximal '{0}' Zeichen enthalten", MaxLength);
                    }
                    else if (lines.Count() > this.MaxRows)
                    {
                        errorText = String.Format("Der Text darf maximal '{0}' Zeilen enthalten", MaxRows);
                    }

                }

                this.Error = errorText;

                return errorText;
            }
        }

        private string error;
        public string Error
        {
            get { return error; }
            private set
            {
                error = value;
                OnPropertyChanged();
            }
        }
    }
}
