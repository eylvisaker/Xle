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

			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine();
			g.AddBottomCentered("Gold:  + " + gold.ToString(), XleColor.Yellow);
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine();

			player.Gold += gold;

			SoundMan.PlaySound(LotaSound.VeryGood);
			XleCore.FlashHPWhileSound(XleColor.Yellow);
		}
	}
}
