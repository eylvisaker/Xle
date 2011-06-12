using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.MuseumDisplays
{
	class NativeCurrency : Exhibit
	{
		public NativeCurrency() : base("Native Currency", Coin.Topaz) { }
		public override int ExhibitID { get { return 7; } }

		public override void PlayerXamine(Player player)
		{
			base.PlayerXamine(player);

			int gold = XleCore.random.Next(1500, 2500);

			g.AddBottom();
			g.AddBottom();
			g.AddBottomCentered("Gold:  + " + gold.ToString());
			g.AddBottom();
			g.AddBottom();

			player.Gold += gold;

			XleCore.FlashHPWhileSound(XleColor.Yellow);
		}
	}
}
