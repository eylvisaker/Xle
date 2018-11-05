using AgateLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Xle.Rendering
{
    public interface IMenuRenderer : IRenderer
    {
        void DrawMenu(SpriteBatch spriteBatch, SubMenu menu);

        SubMenu Menu { get; set; }
    }

    [Transient]
    public class MenuRenderer : IMenuRenderer
    {
        private GameState gameState;
        private IXleRenderer renderer;
        private readonly ITextRenderer textRenderer;
        private readonly IRectangleRenderer rects;

        public MenuRenderer(GameState gameState,
                            IXleRenderer renderer,
                            ITextRenderer textRenderer,
                            IRectangleRenderer rects)
        {
            this.gameState = gameState;
            this.renderer = renderer;
            this.textRenderer = textRenderer;
            this.rects = rects;
        }

        public ColorScheme ColorScheme => gameState.Map.ColorScheme;

        public SubMenu Menu { get; set; }

        /// <summary>
        /// Draws the submenu created by SubMenu.
        /// </summary>
        /// <param name="menu"></param>
        public void DrawMenu(SpriteBatch spriteBatch, SubMenu menu)
        {
            string thestring;
            int xx, yy, i = 0, height;
            string buffer;
            Color fontColor = gameState.Map.ColorScheme.TextColor;

            xx = 624 - menu.width * 16;
            yy = 16;
            height = (menu.theList.Count + 3) * 16;

            var vertLine = gameState.Map.ColorScheme.VerticalLinePosition;

            if (xx < vertLine + 16)
            {
                xx = vertLine + 16;
                i = 1;
            }

            rects.Fill(spriteBatch, new Rectangle(xx, yy, 624 - xx, height), menu.BackColor);

            if (i == 0)
            {
                xx += 16;
            }

            thestring = menu.title;

            textRenderer.WriteText(spriteBatch,
                                   xx + (624 - xx) / 32 * 16 - thestring.Length / 2 * 16,
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

                textRenderer.WriteText(spriteBatch, xx, yy, thestring);

                if (i == menu.value)
                {
                    int xx1;

                    xx1 = xx + thestring.Length * 16;
                    textRenderer.WriteText(spriteBatch, xx1, yy, "`");
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            renderer.Draw(spriteBatch);
            DrawMenu(spriteBatch, Menu);
        }

        public void Update(GameTime time)
        {
        }
    }
}
