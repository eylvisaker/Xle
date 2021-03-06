﻿using AgateLib;
using Microsoft.Xna.Framework.Graphics;

namespace Xle.Rendering
{
    public interface IXleImages
    {
        void LoadTiles(string tileset);

        Texture2D Tiles { get; }
    }

    [Singleton]
    public class XleImages : IXleImages
    {
        private readonly IContentProvider content;

        public XleImages(IContentProvider content)
        {
            this.content = content;
        }

        public Texture2D Tiles { get; private set; }

        public void LoadTiles(string tileset)
        {
            Tiles = content.Load<Texture2D>("Images/" + tileset);
        }
    }
}
