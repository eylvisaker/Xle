using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.MapExtenders.Museum.MuseumDisplays
{
	class NativeCurrency : LotaExhibit
	{
		public NativeCurrency() : base("Native Currency", Coin.Topaz) { }
		public override ExhibitIdentifier ExhibitIdentifier { get { return ExhibitIdentifier.NativeCurrency; } }

		public override void RunExhibit(Player player)
		{
			base.RunExhibit(player);

			int gold = XleCore.random.Next(1500, 2500);

			g.AddBottom();
			g.AddBottom();
			g.AddBottomCentered("Gold:  + " + gold.ToString(), XleColor.Yellow);
			g.AddBottom();
			g.AddBottom();

			player.Gold += gold;

			SoundMan.PlaySound(LotaSound.VeryGood);
			XleCore.FlashHPWhileSound(XleColor.Yellow);
		}
	}
}
