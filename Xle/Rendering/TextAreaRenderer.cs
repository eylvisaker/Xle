﻿
using AgateLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Xle.ScreenModel;

namespace Xle.Rendering
{
    public interface ITextAreaRenderer
    {
        void Draw(SpriteBatch spriteBatch, ITextArea TextArea);
    }

    [Singleton]
    public class TextAreaRenderer : ITextAreaRenderer
    {
        private ITextRenderer textRenderer;

        public TextAreaRenderer(ITextRenderer textRenderer)
        {
            this.textRenderer = textRenderer;
        }

        public void Draw(SpriteBatch spriteBatch, ITextArea textArea)
        {
            for (int i = 0; i < 5; i++)
            {
                //int x = 16 + 16 * g.BottomMargin(i);

                //DrawText(x, 368 - 16 * i, g.Bottom(i), g.BottomColor(i));
                int x = 16;

                var line = textArea.GetLine(i);

                DrawText(spriteBatch, x, 304 + 16 * i, line.Text, line.Colors);
            }
        }

        private void DrawText(SpriteBatch spriteBatch, int x, int y, string text, Color[] color)
        {
            textRenderer.WriteText(spriteBatch, x, y, text, color);
        }

    }
}
