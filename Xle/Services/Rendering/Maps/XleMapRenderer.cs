using AgateLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Xle.Maps;

namespace Xle.Services.Rendering.Maps
{
    public interface IXleMapRenderer
    {

    }

    [InjectProperties]
    public class XleMapRenderer : IXleMapRenderer
    {
        private XleMap mMap;
        private MapExtender mExtender;

        public GameState GameState { get; set; }
        public Random Random { get; set; }
        public IXleRenderer Renderer { get; set; }
        public IRectangleRenderer RectRenderer { get; set; }

        public virtual XleMap TheMap
        {
            get { return mMap; }
            set
            {
                mMap = value;
                OnMapSet();
            }
        }
        public MapExtender Extender
        {
            get { return mExtender; }
            internal set
            {
                mExtender = value;
                OnExtenderSet();
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch, Point playerPos,
            Direction faceDirection, Rectangle inRect)
        {
        }

        protected virtual void OnMapSet()
        {
        }
        protected virtual void OnExtenderSet()
        {
        }

        protected void FillRect(SpriteBatch spriteBatch,
            int x, int y, int width, int height, Color color)
        {
            RectRenderer.Fill(spriteBatch, new Rectangle(x, y, width, height), color);
        }

        protected void FillRect(SpriteBatch spriteBatch, Rectangle rect, Color color)
        {
            RectRenderer.Fill(spriteBatch, rect, color);
        }

        public virtual void Update(GameTime time)
        {

        }
    }
}
