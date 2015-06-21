using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.Services.Implementation
{
    public class TextRenderer : ITextRenderer
    {
        public IXleGameFactory Factory { get; set; }


        public void WriteText(int px, int py, string theText)
        {
            WriteText(px, py, theText, XleColor.White);
        }
        public void WriteText(int px, int py, string theText, Color c)
        {
            if (string.IsNullOrEmpty(theText)) return;

            int i, len = theText.Length + 1;
            Color[] coloring = new Color[len];

            for (i = 0; i < len; i++)
            {
                coloring[i] = c;
            }

            WriteText(px, py, theText, coloring);
        }

        /// <summary>
        /// Writes out the specified text to the back buffer.
        /// </summary>
        /// <param name="px"></param>
        /// <param name="py"></param>
        /// <param name="theText"></param>
        /// <param name="coloring"></param>
        public void WriteText(int px, int py, string theText, Color[] coloring)
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
                fy = (int)(c / 16) * 16;

                Factory.Font.Color = color;

                Factory.Font.DrawText(px, py, c.ToString());
            }
        }

    }
}
