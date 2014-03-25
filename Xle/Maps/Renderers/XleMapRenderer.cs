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
		public virtual XleMap TheMap { get; set; }
		public MapExtender Extender { get; set; }

		public virtual void Draw(Point playerPos, Direction faceDirection, Rectangle inRect)
		{
			XleCore.GameState.Map.Draw(playerPos.X, playerPos.Y, faceDirection, inRect);
		}
	}
}
