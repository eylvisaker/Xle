using AgateLib.Geometry;
using ERY.Xle.Maps;
using ERY.Xle.Maps.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Maps.Renderers
{
	public class XleMapRenderer
	{
		XleMap mMap;
		MapExtender mExtender;

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
			set
			{
				mExtender = value;
				OnExtenderSet();
			}
		}

		public virtual void Draw(Point playerPos, Direction faceDirection, Rectangle inRect)
		{
			XleCore.GameState.Map.Draw(playerPos.X, playerPos.Y, faceDirection, inRect);
		}

		protected virtual void OnMapSet()
		{
		}
		protected virtual void OnExtenderSet()
		{
		}
	}
}
