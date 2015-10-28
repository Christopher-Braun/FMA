using System.Windows;
using System.Windows.Media;

namespace FMA.Core
{
    public class TextField
    {
        public TextField(FormattedText formattedText, Point origin)
        {
            FormattedText = formattedText;
            Origin = origin;
        }

        public FormattedText FormattedText { get; private set; }

        public Point Origin { get; private set; }
    }
}