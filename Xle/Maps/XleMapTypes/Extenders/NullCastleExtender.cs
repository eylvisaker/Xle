using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Maps.XleMapTypes.Extenders
{
	public class NullCastleExtender : NullTownExtender, ICastleExtender
	{
		public new Castle TheMap { get { return (Castle)base.TheMap; } }

		public override void SetColorScheme(ColorScheme scheme)
		{
			scheme.TextColor = XleColor.White;

			scheme.FrameColor = XleColor.Gray;
			scheme.FrameHighlightColor = XleColor.Yellow;

			scheme.VerticalLinePosition = 13 * 16;
		}

		public override void SpeakToGuard(GameState state, ref bool handled)
		{
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("The guard ignores you.");

			handled = true;
		}

	}
}
