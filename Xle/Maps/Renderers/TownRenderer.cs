using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Maps.Renderers
{
	public class TownRenderer : Map2DRenderer
	{
		protected override void Animate(AgateLib.Geometry.Rectangle tileRect)
		{
			TheMap.Guards.AnimateGuards();
		}
	}
}
