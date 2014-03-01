﻿using ERY.Xle.XleEventTypes;
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
		public new Outside TheMap { get { return (Outside)base.TheMap; } }

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

		public override void SetColorScheme(ColorScheme scheme)
		{
			scheme.TextColor = XleColor.White;

			scheme.FrameColor = XleColor.Brown;
			scheme.FrameHighlightColor = XleColor.Yellow;

			scheme.VerticalLinePosition = 15 * 16;
		}
	}
}
