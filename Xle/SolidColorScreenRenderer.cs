using System;
using System.Collections.Generic;
using System.Text;
using AgateLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Xle
{
    [Transient, InjectProperties]
    public class SolidColorScreenRenderer : IRenderer
    {
        public IRectangleRenderer RectangleRenderer { get; set; }

        public Color Color { get; set; } = XleColor.Gray;

        public void Draw(SpriteBatch spriteBatch)
        {
            RectangleRenderer.Fill(spriteBatch, new Rectangle(-50, -50, 1000, 1000), Color);
        }

        public void Update(GameTime time)
        {

        }
    }
}
