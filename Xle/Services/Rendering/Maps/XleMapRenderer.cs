using System;

using AgateLib.Mathematics.Geometry;
using AgateLib.Mathematics.Geometry;
using ERY.Xle.Maps;

namespace ERY.Xle.Services.Rendering.Maps
{
	public class XleMapRenderer
	{
		XleMap mMap;
		MapExtender mExtender;

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

		public virtual void Draw(Point playerPos, Direction faceDirection, Rectangle inRect)
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
