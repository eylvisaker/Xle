using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes.Stores.Extenders
{
	public class StoreHealer : StoreFrontExtender
	{
		bool buyHerbs = false;

		protected override void SetColorScheme(ColorScheme cs)
		{
			cs.BackColor = buyHerbs ? XleColor.LightBlue : XleColor.Green;
			cs.FrameColor = XleColor.LightGreen;
			cs.FrameHighlightColor = XleColor.Yellow;
			cs.BorderColor = XleColor.Gray;
		}
	}
}
