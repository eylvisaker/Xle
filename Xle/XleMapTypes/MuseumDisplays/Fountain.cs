using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.MuseumDisplays
{
	class Fountain : Exhibit
	{
		public Fountain() : base("A Fountain", Coin.Jade) { }
		public override int ExhibitID { get { return 4; } }
		public override string LongName
		{
			get
			{
				return "Enchanted flower fountain";
			}
		}

		public override bool IsClosed(ERY.Xle.Player player)
		{
			if (player.museum[ExhibitID] == 7)
				return true;
			else
				return false;
		}

		public override void PlayerXamine(Player player)
		{
			if (player.Item(10) == 0)
			{
				base.PlayerXamine(player);
				g.AddBottom();

				if (player.museum[ExhibitID] == 0 || player.museum[ExhibitID] == 1)
					g.AddBottom("Do you want to help search?");
				else
					g.AddBottom("Do you want to continue searching?");


				g.AddBottom();
				if (XleCore.QuickMenuYesNo() == 0)
				{
					ReadRawText(ExhibitInfo.Text[2]);
					int amount = player.TimeDays < 100 ? 100 : 300;
					
					player.Gold += amount;

					g.UpdateBottom("            Gold:  + " + amount.ToString(), XleColor.Yellow);

					SoundMan.PlaySound(LotaSound.VeryGood);
					XleCore.FlashHPWhileSound(XleColor.Yellow);

					XleCore.WaitForKey();

					player.museum[ExhibitID] = 3;
				}
				else
				{
					player.museum[ExhibitID] = 1;
				}
			}
			else
			{
				// remove the tulip from the player, give the reward and shut down the exhibit.
				player.ItemCount(10, -player.Item(10));
				player.Attribute[Attributes.charm] += 10;
				player.museum[ExhibitID] = 7;

				ReadRawText(ExhibitInfo.Text[3]);

			}
		}
	}
}
