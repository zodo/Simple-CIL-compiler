namespace Language.GUI
{
    using System.Collections.Generic;
    using System.Drawing;

    using FastColoredTextBoxNS;

    public class ColorScheme
    {
        public static Dictionary<string, TextStyle> Colors = new Dictionary<string, TextStyle>
        {
            {"bold", new TextStyle(Brushes.Black, null, FontStyle.Bold) },
            {"italic", new TextStyle(Brushes.Black, null, FontStyle.Italic) },
            {"boldBlue", new TextStyle(Brushes.DarkBlue, null, FontStyle.Bold) },
            {"fade", new TextStyle(Brushes.DimGray, null, FontStyle.Regular) },
            {"string", new TextStyle(Brushes.Chocolate, null, FontStyle.Regular) },
            {"func", new TextStyle(Brushes.Turquoise, null, FontStyle.Bold) },
            {"comment" , new TextStyle(Brushes.LimeGreen, null, FontStyle.Italic)},
            {"error", new TextStyle(Brushes.Black, Brushes.LightPink, FontStyle.Regular) }
        }; 
    }
}
