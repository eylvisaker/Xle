using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.Extenders
{
	public class NullCastleExtender : NullMapExtender, ICastleExtender
	{
		public override void GetBoxColors(out AgateLib.Geometry.Color boxColor, out AgateLib.Geometry.Color innerColor, out AgateLib.Geometry.Color fontColor, out int vertLine)
		{
			fontColor = XleColor.White;

			boxColor = XleColor.Orange;
			innerColor = XleColor.Yellow;

			vertLine = 13 * 16; 
		}
	}
}
