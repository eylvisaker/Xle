using AgateLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Xle
{
    public interface IRectangleRenderer
    {
        void Fill(SpriteBatch spriteBatch, Rectangle rectangle, Color color);
    }

    [Singleton]
    public class RectangleRenderer : IRectangleRenderer
    {
        private readonly Texture2D white;

        public RectangleRenderer(IContentProvider content)
        {
            white = content.Load<Texture2D>("white");
        }

        public void Fill(SpriteBatch spriteBatch, Rectangle rectangle, Color color)
        {
            spriteBatch.Draw(white, rectangle, color);
        }
    }
}
