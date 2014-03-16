using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.MapExtenders.Museum.MuseumDisplays
{
	class Thornberry : LotaExhibit
	{
		public Thornberry() : base("Thornberry", Coin.Jade) { }
		public override ExhibitIdentifier ExhibitIdentifier { get { return ExhibitIdentifier.Thornberry; } }
		public override string LongName
		{
			get { return "A typical town of Tarmalon"; }
		}

		public override void RunExhibit(Player player)
		{
			if (CheckOfferReread(player))
			{
				ReadRawText(ExhibitInfo.Text[1]);
			}

			XleCore.TextArea.PrintLine("Would you like to go");
			XleCore.TextArea.PrintLine("to thornberry?");
			XleCore.TextArea.PrintLine();

			if (XleCore.QuickMenu(new MenuItemList("Yes", "no"), 3) == 0)
			{
				ReadRawText(ExhibitInfo.Text[2]);

				int amount = 100;

				if (HasBeenVisited(player) ||
					HasBeenVisited(player, ExhibitIdentifier.Fountain))
				{
					amount += 200;
				}

				player.Gold += amount;

				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine("             GOLD:  + " + amount.ToString(), XleColor.Yellow);

				SoundMan.PlaySound(LotaSound.VeryGood);
				XleCore.FlashHPWhileSound(XleColor.Yellow);

				XleCore.WaitForKey();

				XleCore.ChangeMap(player, 11, 0);
				player.SetReturnLocation(1, 18, 56);
			}
		}
	}
}
