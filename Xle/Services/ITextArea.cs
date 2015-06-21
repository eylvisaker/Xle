using System;
using AgateLib.Geometry;
using ERY.Xle.Services.Implementation;

namespace ERY.Xle.Services
{
    public interface ITextArea : IXleService
    {
        Point CursorLocation { get; }

        void Clear(bool setCursorAtTop = false);

        void PrintLine(string text, Color color);
        void PrintLine(string text = "", Color[] colors = null);

        void Print(string text, Color? color);
        void Print(string text = "", Color[] colors = null);

        void PrintSlow(string text, Color color);
        void PrintSlow(string text, Color[] colors = null);

        void RewriteLine(int line, string text, Color? color = null);

        void PrintLineSlow(string text, Color color);
        void PrintLineSlow(string text = "", Color[] colors = null);

        void FlashLines(int howLong, Color color, int flashRate, params int[] lines);
        void FlashLinesWhile(Func<bool> pred, Color color1, Color color2, int flashRate, params int[] lines);

        void SetLineColor(Color color, params int[] lines);

        void SetCharacterColor(int p1, int p2, Color color);

        int Margin { get; set; }


        void PrintLineCentered(string p, Color color);

        TextLine GetLine(int i);
    }
}
