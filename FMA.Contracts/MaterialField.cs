using System.Diagnostics;

namespace FMA.Contracts
{
    [DebuggerDisplay("Name: {FieldName} Value: {Value}")]
    public class MaterialField
    {
        public MaterialField(string fieldName, string fontName, int fontSize, bool bold, bool italic, bool uppper, int maxLength, int maxRows, int leftMargin, int topMargin, string value, string textColor = "Black")
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
            TextColor = textColor;
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
        public string Value { get; set; }
        public string TextColor { get; set; }
    }
}
