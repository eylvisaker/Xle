using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ERY.Xle.Services.Implementation;

namespace ERY.Xle.LotA.MapExtenders.Museum.MuseumDisplays
{
	class NativeCurrency : LotaExhibit
	{
		public NativeCurrency() : base("Native Currency", Coin.Topaz) { }
		public override ExhibitIdentifier ExhibitIdentifier { get { return ExhibitIdentifier.NativeCurrency; } }

		public override void RunExhibit(Player player)
		{
			base.RunExhibit(player);

			int gold = (int)(350 * (1 + player.Level) 
				* (1 + XleCore.random.NextDouble()));

			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("             Gold:  + " + gold.ToString(), XleColor.Yellow);
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine();

			player.Gold += gold;

			SoundMan.PlaySound(LotaSound.VeryGood);
			XleCore.FlashHPWhileSound(XleColor.Yellow);
		}
	}
}
