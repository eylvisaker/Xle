using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.Extenders
{
	public class NullTownExtender : NullMapExtender, ITownExtender
	{
		public override void GetBoxColors(out Color boxColor, out Color innerColor, out Color fontColor, out int vertLine)
		{

			fontColor = XleColor.White;

			boxColor = XleColor.Orange;
			innerColor = XleColor.Yellow;
			vertLine = 13 * 16;			
		}

		public virtual void SpeakToGuard(GameState state, ref bool handled)
		{
		}


		public virtual void SetAngry(bool value)
		{
		}
	}
}
