using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Outside
{
	class Thalen : LobBaseOutside
	{
		public override void SetColorScheme(ColorScheme scheme)
		{
			base.SetColorScheme(scheme);

			scheme.FrameColor = XleColor.Gray;
			scheme.FrameHighlightColor = XleColor.Green;
			scheme.TextColor = XleColor.White;

			scheme.VerticalLinePosition = 13 * 16;
		}
	}
}
