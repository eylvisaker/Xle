using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.Extenders
{
	class NullOutsideExtender : IOutsideExtender
	{
		public virtual int GetOutsideTile(AgateLib.Geometry.Point playerPoint, int x, int y)
		{
			return 0;
		}

		public virtual void GetBoxColors(out AgateLib.Geometry.Color boxColor, out AgateLib.Geometry.Color innerColor, out AgateLib.Geometry.Color fontColor, out int vertLine)
		{

			fontColor = XleColor.White;


			boxColor = XleColor.Brown;
			innerColor = XleColor.Yellow;
			vertLine = 15 * 16;

		}
	}
}
