using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes.Stores
{
	public class StoreHealer : StoreFront
	{
		bool buyHerbs = false;

		public StoreHealer()
		{
			ExtenderName = "Healer";
		}

		protected override void ReadData(AgateLib.Serialization.Xle.XleSerializationInfo info)
		{
			base.ReadData(info);
			ExtenderName = "Healer";
		}

		protected override void SetColorScheme(ColorScheme cs)
		{
			cs.BackColor = buyHerbs ? XleColor.LightBlue : XleColor.Green;
			cs.FrameColor = XleColor.LightGreen;
			cs.FrameHighlightColor = XleColor.Yellow;
			cs.BorderColor = XleColor.Gray;
		}
	}
}
