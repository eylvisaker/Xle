using ERY.Xle.Maps.XleMapTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Maps.Extenders
{
	public class CastleExtender : TownExtender
	{
		public new Castle TheMap { get { return (Castle)base.TheMap; } }

		public override void SetColorScheme(ColorScheme scheme)
		{
			scheme.TextColor = XleColor.White;

			scheme.FrameColor = XleColor.Gray;
			scheme.FrameHighlightColor = XleColor.Yellow;
		}

		public override void SpeakToGuard(GameState state)
		{
			XleCore.TextArea.PrintLine("\n\nThe guard ignores you.");
		}


		public override void PlayOpenRoofSound(Roof roof)
		{
			// do nothing here
		}
		public override void PlayCloseRoofSound(Roof roof)
		{
			// do nothing here
		}
	}
}
