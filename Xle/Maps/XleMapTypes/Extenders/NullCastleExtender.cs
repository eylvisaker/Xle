﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.Extenders
{
	public class NullCastleExtender : NullMapExtender, ICastleExtender
	{
		public new Castle TheMap { get { return (Castle)base.TheMap; } }

		public override void GetBoxColors(out AgateLib.Geometry.Color boxColor, out AgateLib.Geometry.Color innerColor, out AgateLib.Geometry.Color fontColor, out int vertLine)
		{
			fontColor = XleColor.White;

			boxColor = XleColor.Gray;
			innerColor = XleColor.Yellow;

			vertLine = 13 * 16; 
		}

		public virtual void SpeakToGuard(GameState state, ref bool handled)
		{
			g.AddBottom("");
			g.AddBottom("The guard ignores you.");

			handled = true;
		}


		public virtual void SetAngry(bool value)
		{
		}
	}
}
