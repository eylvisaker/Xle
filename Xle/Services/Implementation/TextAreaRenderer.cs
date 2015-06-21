﻿using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.Services.Implementation
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
