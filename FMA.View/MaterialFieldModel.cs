using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FMA.Contracts.Properties;

namespace FMA.View
{
    [DebuggerDisplay("Name: {FieldName} Value {Value}")]
    public class MaterialFieldModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private string value;
        private string error;
        private int topMargin;
        private int leftMargin;
        private int maxRows;
        private int maxLength;
        private bool uppper;
        private bool italic;
        private bool bold;
        private int fontSize;
        private string fontName;
        private string fieldName;

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

        public string FieldName
        {
            get { return fieldName; }
            set
            {
                fieldName = value;
                OnPropertyChanged();
            }
        }

        public string FontName
        {
            get { return fontName; }
            set
            {
                fontName = value;
                OnPropertyChanged();
            }
        }

        public int FontSize
        {
            get { return fontSize; }
            set
            {
                fontSize = value;
                OnPropertyChanged();
            }
        }

        public bool Bold
        {
            get { return bold; }
            set
            {
                bold = value;
                OnPropertyChanged();
            }
        }

        public bool Italic
        {
            get { return italic; }
            set
            {
                italic = value;
                OnPropertyChanged();
            }
        }

        public bool Uppper
        {
            get { return uppper; }
            set
            {
                uppper = value;
                OnPropertyChanged();
            }
        }

        public int MaxLength
        {
            get { return maxLength; }
            set
            {
                maxLength = value;
                OnPropertyChanged();
            }
        }

        public int MaxRows
        {
            get { return maxRows; }
            set
            {
                maxRows = value;
                OnPropertyChanged();
            }
        }

        public int LeftMargin
        {
            get { return leftMargin; }
            set
            {
                leftMargin = value;
                OnPropertyChanged();
            }
        }

        public int TopMargin
        {
            get { return topMargin; }
            set
            {
                topMargin = value;
                OnPropertyChanged();
            }
        }


        public string Value
        {
            get { return value; }
            set
            {
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
                    var newLine = new[] { "\r\n" };
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



        public string Error
        {
            get { return error; }
            private set
            {
                error = value;
                OnPropertyChanged();
            }
        }

        public static Action<MaterialFieldModel> EditLayoutMode;


        public void MouseDoubleClick()
        {
            if (Keyboard.Modifiers == ModifierKeys.Shift)
            {
                EditLayoutMode(this);
            }
        }
    }
}
