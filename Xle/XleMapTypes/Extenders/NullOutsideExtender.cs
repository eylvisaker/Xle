using ERY.Xle.XleEventTypes;
using ERY.Xle.XleEventTypes.Extenders;
using ERY.Xle.XleEventTypes.Extenders.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.Extenders
{
	public class NullOutsideExtender : NullMapExtender, IOutsideExtender
	{
		public override int GetOutsideTile(AgateLib.Geometry.Point playerPoint, int x, int y)
		{
			return 0;
		}

		public override IEventExtender CreateEventExtender(XleEvent evt, Type defaultExtender)
		{
			if (evt is ChangeMapEvent)
				return new ChangeMapQuestion();
			else
				return base.CreateEventExtender(evt, defaultExtender);
		}

		public override void GetBoxColors(out AgateLib.Geometry.Color boxColor, out AgateLib.Geometry.Color innerColor, out AgateLib.Geometry.Color fontColor, out int vertLine)
		{

			fontColor = XleColor.White;


			boxColor = XleColor.Brown;
			innerColor = XleColor.Yellow;
			vertLine = 15 * 16;

		}
	}
}
