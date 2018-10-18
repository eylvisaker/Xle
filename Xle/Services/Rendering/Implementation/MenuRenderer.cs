using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Xle;

namespace ERY.Xle.Services.Rendering.Implementation
{
    public class MenuRenderer : IMenuRenderer
    {
        private GameState GameState;
        private IXleRenderer Renderer;
        private readonly IRectangleRenderer rects;

        public MenuRenderer(GameState gameState, IXleRenderer renderer, IRectangleRenderer rects)
        {
            this.GameState = gameState;
            this.Renderer = renderer;
            this.rects = rects;
        }

        public ITextRenderer TextRenderer { get; set; }

        /// <summary>
        /// Draws the submenu created by SubMenu.
        /// </summary>
        /// <param name="menu"></param>
        public void DrawMenu(SpriteBatch spriteBatch, SubMenu menu)
        {
            string thestring;
            int xx, yy, i = 0, height;
            string buffer;
            Color fontColor = GameState.Map.ColorScheme.TextColor;

            xx = 624 - menu.width * 16;
            yy = 16;
            height = (menu.theList.Count + 3) * 16;

            var vertLine = GameState.Map.ColorScheme.VerticalLinePosition;

            if (xx < vertLine + 16)
            {
                xx = vertLine + 16;
                i = 1;
            }

            rects.Fill(spriteBatch, new Rectangle(xx, yy, 624-xx, height), menu.BackColor);

            if (i == 0)
            {
                xx += 16;
            }

            thestring = menu.title;

            TextRenderer.WriteText(spriteBatch, 
                                   xx + (int)((624 - xx) / 32) * 16 - (int)(thestring.Length / 2) * 16,
                                   yy, 
                                   thestring, 
                                   fontColor);

            yy += 16;

            for (i = 0; i < menu.theList.Count; i++)
            {
                yy += 16;
                buffer = menu.theList[i];

                if (i > 9)
                    thestring = ((char)(i + 'A' - 10)).ToString();
                else
                    thestring = i.ToString();

                thestring += ". " + buffer;

                TextRenderer.WriteText(spriteBatch, xx, yy, thestring);

                if (i == menu.value)
                {
                    int xx1;

                    xx1 = xx + thestring.Length * 16;
                    TextRenderer.WriteText(spriteBatch, xx1, yy, "`");
                }
            }
        }
    }
}
