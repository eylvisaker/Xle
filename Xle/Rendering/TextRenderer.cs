﻿
using AgateLib;
using Xle.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Xle.Rendering
{
    public interface ITextRenderer
    {
        void WriteText(SpriteBatch spriteBatch, int x, int y, string text, Color[] color);

        void WriteText(SpriteBatch spriteBatch, int px, int py, string theText);

        void WriteText(SpriteBatch spriteBatch, int px, int py, string theText, Color c);
    }

    [Singleton, InjectProperties]
    public class TextRenderer : ITextRenderer
    {
        public IXleGameFactory Factory { get; set; }

        public void WriteText(SpriteBatch spriteBatch, int px, int py, string theText)
        {
            WriteText(spriteBatch, px, py, theText, XleColor.White);
        }

        public void WriteText(SpriteBatch spriteBatch, int px, int py, string theText, Color c)
        {
            if (string.IsNullOrEmpty(theText)) return;

            int i, len = theText.Length + 1;
            Color[] coloring = new Color[len];

            for (i = 0; i < len; i++)
            {
                coloring[i] = c;
            }

            WriteText(spriteBatch, px, py, theText, coloring);
        }

        /// <summary>
        /// Writes out the specified text to the back buffer.
        /// </summary>
        /// <param name="px"></param>
        /// <param name="py"></param>
        /// <param name="theText"></param>
        /// <param name="coloring"></param>
        public void WriteText(SpriteBatch spriteBatch, int px, int py, string theText, Color[] coloring)
        {
            if (string.IsNullOrEmpty(theText))
                return;

            int i;
            int fx, fy;
            int startx = px;

            int len = theText.Length;
            Color color;

            for (i = 0; i < len; i++, px += 16)
            {
                char c = theText[i];

                if (c == '\n')
                {
                    px = startx - 16;
                    py += 16;
                }

                if (coloring != null)
                {
                    if (i < coloring.Length)
                        color = coloring[i];
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Warning: coloring array was too short.");
                        color = coloring[coloring.Length - 1];
                    }
                }
                else
                {
                    color = XleColor.White;
                }

                ///  removed the new graphics because colored message looks like crap.  I need to 
                ///	 antialias it some other way
                fx = c % 16 * 16;//+ 256 * g.newGraphics;
                fy = c / 16 * 16;

                Factory.Font.Color = color;

                Factory.Font.DrawText(spriteBatch, new Vector2(px, py), c.ToString());
            }
        }

    }
}
