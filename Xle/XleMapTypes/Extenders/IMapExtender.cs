using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.Extenders
{
	public interface IMapExtender
	{
		int GetOutsideTile(Point playerPoint, int x, int y);



		void GetBoxColors(out Color boxColor, out Color innerColor, out Color fontColor, out int vertLine);
	}
}
