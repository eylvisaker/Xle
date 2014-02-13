using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.Extenders
{
	public class NullMapExtender : IMapExtender
	{
		public virtual int GetOutsideTile(Point playerPoint, int x, int y)
		{
			return -1;
		}


		public virtual void GetBoxColors(out Color boxColor, out Color innerColor, out Color fontColor, out int vertLine)
		{
			throw new NotImplementedException();
		}
	}
}
