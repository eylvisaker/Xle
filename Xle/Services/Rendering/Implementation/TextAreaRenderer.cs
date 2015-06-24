using AgateLib.Geometry;

using ERY.Xle.Services.ScreenModel;

namespace ERY.Xle.Services.Rendering.Implementation
{
    public class TextAreaRenderer : ITextAreaRenderer
    {
        public ITextRenderer TextRenderer { get; set; }

        public void Draw(ITextArea textArea)
        {
            for (int i = 0; i < 5; i++)
            {
                //int x = 16 + 16 * g.BottomMargin(i);

                //DrawText(x, 368 - 16 * i, g.Bottom(i), g.BottomColor(i));
                int x = 16;

                var line = textArea.GetLine(i);

                DrawText(x, 304 + 16 * i, line.Text, line.Colors);
            }
        }

        private void DrawText(int x, int y, string text, Color[] color)
        {
            TextRenderer.WriteText(x, y, text, color);
        }

    }
}
