using AgateLib.DisplayLib;

namespace ERY.Xle.Services.Rendering
{
    public interface ITextRenderer : IXleService
    {
        void WriteText(int x, int y, string text, Color[] color);

        void WriteText(int px, int py, string theText);

        void WriteText(int px, int py, string theText, Color c);
    }
}
