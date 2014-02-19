using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.MuseumDisplays
{
	class Thornberry : Exhibit
	{
		public Thornberry() : base("Thornberry", Coin.Jade) { }
		public override ExhibitIdentifier ExhibitID { get { return ExhibitIdentifier.Thornberry; } }
		public override string LongName
		{
			get { return "A typical town of Tarmalon"; }
		}

		public override void PlayerXamine(Player player)
		{
			if (CheckOfferReread(player))
			{
				ReadRawText(ExhibitInfo.Text[1]);
			}

			g.AddBottom("Would you like to go");
			g.AddBottom("to thornberry?");
			g.AddBottom();

			if (XleCore.QuickMenu(new MenuItemList("Yes", "no"), 3) == 0)
			{
				ReadRawText(ExhibitInfo.Text[2]);

				int amount = 100;

				if (player.museum[(int)ExhibitID] > 0 || player.museum[(int)ExhibitIdentifier.Fountain] > 0)
					amount += 200;

				player.Gold += amount;

				g.UpdateBottom("             GOLD:  + " + amount.ToString(), 1, XleColor.Yellow);

				SoundMan.PlaySound(LotaSound.VeryGood);
				XleCore.FlashHPWhileSound(XleColor.Yellow);

				XleCore.WaitForKey();

				player.SetMap(11, 75, 17);
				player.SetReturnLocation(1, 18, 56);
			}
		}
	}
}
