using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.Extenders
{
	public class NullMap2DExtender : IMap2DExtender
	{
		public virtual int GetOutsideTile(Point playerPoint, int x, int y)
		{
			return -1;
		}
	}
}
