using AgateLib.Geometry;
using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.Extenders
{
	public interface IMapExtender
	{
		int GetOutsideTile(Point playerPoint, int x, int y);

		IEventExtender CreateEventExtender(XleEvent evt, Type defaultExtender);

		void GetBoxColors(out Color boxColor, out Color innerColor, out Color fontColor, out int vertLine);
	}
}
