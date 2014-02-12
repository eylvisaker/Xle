using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.Extenders
{
	public interface IMap2DExtender
	{
		int GetOutsideTile(Point playerPoint, int x, int y);
	}
}
