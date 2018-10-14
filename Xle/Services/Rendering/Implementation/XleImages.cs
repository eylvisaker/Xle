using AgateLib;
using Microsoft.Xna.Framework.Graphics;

namespace ERY.Xle.Services.Rendering.Implementation
{
    public class XleImages : IXleImages
    {
        public IContentProvider Content { get; set; }

        public Texture2D Tiles { get; private set; }

        public void LoadTiles(string tileset)
        {
            if (tileset.EndsWith(".png") == false)
                tileset += ".png";

            Tiles = Content.Load<Texture2D>("Images/" + tileset);
        }
    }
}
