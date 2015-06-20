using AgateLib.Geometry;

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

        void RewriteLine(int line, string text, Color? color = null);
    }
}
