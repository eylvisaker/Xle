using Xle.Maps;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Xle.Services.Rendering.Maps
{
    public class XleMapRenderer
    {
        private XleMap mMap;
        private MapExtender mExtender;

        public GameState GameState { get; set; }
        public Random Random { get; set; }
        public IXleRenderer Renderer { get; set; }

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

        public virtual void Draw(GameTime time, SpriteBatch spriteBatch, 
                                 Point playerPos, Direction faceDirection, Rectangle inRect)
        {
        }

        protected virtual void OnMapSet()
        {
        }
        protected virtual void OnExtenderSet()
        {
        }
    }
}
