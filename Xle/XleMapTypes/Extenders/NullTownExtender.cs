using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.Extenders
{
	public class NullTownExtender : ITownExtender
	{
		public virtual int GetOutsideTile(AgateLib.Geometry.Point playerPoint, int x, int y)
		{
			return 0;
		}

		public virtual void GetBoxColors(out Color boxColor, out Color innerColor, out Color fontColor, out int vertLine)
		{

			fontColor = XleColor.White;

			boxColor = XleColor.Orange;
			innerColor = XleColor.Yellow;
			vertLine = 13 * 16;			
		}
	}
}
