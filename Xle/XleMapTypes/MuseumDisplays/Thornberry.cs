using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.MuseumDisplays
{
	class Thornberry : Exhibit
	{
		public Thornberry() : base("Thornberry", Coin.Jade) { }
		public override int ExhibitID { get { return 3; } }
		public override string LongName
		{
			get { return "A typical town of Tarmalon"; }
		}

		public override void PlayerXamine(Player player)
		{
			if (CheckOfferReread(player))
			{
				ReadRawText(XleCore.ExhibitInfo[ExhibitID].Text[1]);
			}

			g.AddBottom("Would you like to go");
			g.AddBottom("to thornberry?");
			g.AddBottom();

			if (XleCore.QuickMenu(new MenuItemList("Yes", "no"), 3) == 0)
			{
				ReadRawText(XleCore.ExhibitInfo[ExhibitID].Text[2]);

				int amount = player.TimeDays < 100 ? 100 : 300;

				player.Gold += amount;

				g.UpdateBottom("             GOLD:  + " + amount.ToString(), 1, XleColor.Yellow);

				SoundMan.PlaySound(LotaSound.VeryGood);
				XleCore.FlashHPWhileSound(XleColor.Yellow);

				XleCore.WaitForKey();

				player.SetMap(11, 75, 17);
				player.SetOutsideLocation(1, 18, 56);
			}
		}
	}
}
