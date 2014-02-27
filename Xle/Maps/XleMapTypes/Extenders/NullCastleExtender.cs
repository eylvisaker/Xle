using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.Extenders
{
	public class NullCastleExtender : NullTownExtender, ICastleExtender
	{
		public new Castle TheMap { get { return (Castle)base.TheMap; } }

		public override void GetBoxColors(out AgateLib.Geometry.Color boxColor, out AgateLib.Geometry.Color innerColor, out AgateLib.Geometry.Color fontColor, out int vertLine)
		{
			fontColor = XleColor.White;

			boxColor = XleColor.Gray;
			innerColor = XleColor.Yellow;

			vertLine = 13 * 16; 
		}

		public override void SpeakToGuard(GameState state, ref bool handled)
		{
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("The guard ignores you.");

			handled = true;
		}

	}
}
