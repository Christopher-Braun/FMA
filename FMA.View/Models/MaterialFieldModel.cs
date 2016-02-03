// Christopher Braun 2016

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using FMA.Contracts;
using FMA.View.Properties;

namespace FMA.View.Models
{
    [DebuggerDisplay("Name: {FieldName}, Value {Value}")]
    public class MaterialFieldModel : MaterialChildModel, IDataErrorInfo
    {
        private string value;
        private string error = string.Empty;
        private int topMargin = 100;
        private int leftMargin = 20;
        private int maxRows = 5;
        private int maxLength = 200;
        private bool uppper;
        private bool italic;
        private bool bold;
        private int fontSize = 12;
        private FontFamilyWithName fontFamilyWithName;
        private string textColor = "Black";

        public MaterialFieldModel(string fieldName, string value, FontFamilyWithName fontFamilyWithName)
        {
            FieldName = fieldName;
            this.value = value;
            FontFamilyWithName = fontFamilyWithName;
        }

        public MaterialFieldModel(string fieldName, string value, FontFamilyWithName fontFamilyWithName, int fontSize,
            bool bold, bool italic, bool uppper, int maxLength, int maxRows, int leftMargin, int topMargin,
            string textColor = "Black")
            : this(fieldName, value, fontFamilyWithName)
        {
            TextColor = textColor;
            FontSize = fontSize;
            Bold = bold;
            Italic = italic;
            Uppper = uppper;
            MaxLength = maxLength;
            MaxRows = maxRows;
            LeftMargin = leftMargin;
            TopMargin = topMargin;
        }

        public double Width => Math.Round(GetFormattedText().Width);
        public double Height => Math.Round(GetFormattedText().Height);

        private FormattedText GetFormattedText()
        {
            var fontStyle = Italic ? FontStyles.Italic : FontStyles.Normal;
            var fontWeight = Bold ? FontWeights.Bold : FontWeights.Normal;

            var typeface = new Typeface(FontFamilyWithName.FontFamily, fontStyle, fontWeight, FontStretches.Normal);

            var formattedText = new FormattedText(DisplayValue, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight,
                typeface, FontSize, Brushes.Black);

            return formattedText;
        }

        public FontFamilyWithName FontFamilyWithName
        {
            get { return fontFamilyWithName; }
            set
            {
                if (Equals(value, FontFamilyWithName))
                {
                    return;
                }

                if (value == null)
                {
                    //Hack weil das Binding an die ComboBox beim switchen der View da null reinsetzt
                    return;
                }

                fontFamilyWithName = value;
                OnPropertyChanged();
                NotifyWidthAndHeightChanged();
            }
        }

        public int FontSize
        {
            get { return fontSize; }
            set
            {
                if (value == FontSize)
                {
                    return;
                }
                fontSize = value;
                OnPropertyChanged();
                NotifyWidthAndHeightChanged();
            }
        }

        public bool Bold
        {
            get { return bold; }
            set
            {
                bold = value;
                OnPropertyChanged();
                NotifyWidthAndHeightChanged();
            }
        }

        public bool Italic
        {
            get { return italic; }
            set
            {
                italic = value;
                OnPropertyChanged();
                NotifyWidthAndHeightChanged();
            }
        }

        public bool Uppper
        {
            get { return uppper; }
            set
            {
                uppper = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DisplayValue));
                NotifyWidthAndHeightChanged();
            }
        }

        private void NotifyWidthAndHeightChanged()
        {
            OnPropertyChanged(nameof(Width));
            OnPropertyChanged(nameof(Height));
        }

        public string TextColor
        {
            get { return textColor; }
            set
            {
                textColor = value;
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
                OnPropertyChanged(nameof(DisplayValue));
                NotifyWidthAndHeightChanged();
            }
        }

        public string DisplayValue => Uppper ? Value.ToUpper() : Value;


        public string this[string columnName]
        {
            get
            {
                var errorText = string.Empty;

                if (columnName == nameof(Value))
                {
                    var newLine = new[] {"\r\n"};
                    var lines = Value.Split(newLine, StringSplitOptions.None);

                    //if (lines.Aggregate((s1, s2) => s1 + s2).Length > MaxLength)
                    //{
                    //    errorText = String.Format("Der Text darf maximal '{0}' Zeichen enthalten", MaxLength);
                    //} 
                    if (lines.Any(t => t.Length > MaxLength))
                    {
                        errorText = string.Format(Resources.MaxLenghtHint, MaxLength);
                    }
                    else if (lines.Length > MaxRows)
                    {
                        errorText = string.Format(Resources.MaxLinesHint, MaxRows);
                    }
                }

                Error = errorText;

                return errorText;
            }
        }

        public string Error
        {
            get { return error; }
            private set
            {
                if (string.CompareOrdinal(error, value) == 0)
                {
                    return;
                }
                error = value;
                OnPropertyChanged();
            }
        }
    }
}