using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.MuseumDisplays
{
	class Fountain : Exhibit
	{
		public Fountain() : base("A Fountain", Coin.Jade) { }
		public override ExhibitIdentifier ExhibitID { get { return ExhibitIdentifier.Fountain; } }
		public override string LongName
		{
			get
			{
				return "Enchanted flower fountain";
			}
		}

		public override bool IsClosed(ERY.Xle.Player player)
		{
			if (player.museum[(int)ExhibitID] == 7)
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

				if (player.museum[(int)ExhibitID] == 0 || player.museum[(int)ExhibitID] == 1)
					g.AddBottom("Do you want to help search?");
				else
					g.AddBottom("Do you want to continue searching?");


				g.AddBottom();
				if (XleCore.QuickMenuYesNo() == 0)
				{
					ReadRawText(ExhibitInfo.Text[2]);
					int amount = player.museum[3] != 0 ? 300 : 100;
					
					player.Gold += amount;

					//g.UpdateBottom("            Gold:  + " + amount.ToString(), XleColor.Yellow);
					XleCore.TextArea.PrintLine();
					XleCore.TextArea.PrintLine("            Gold:  + " + amount.ToString(), XleColor.Yellow);

					SoundMan.PlaySound(LotaSound.VeryGood);
					XleCore.FlashHPWhileSound(XleColor.Yellow);

					XleCore.WaitForKey();

					player.museum[(int)ExhibitID] = 3;
				}
				else
				{
					player.museum[(int)ExhibitID] = 1;
				}
			}
			else
			{
				// remove the tulip from the player, give the reward and shut down the exhibit.
				player.ItemCount(10, -player.Item(10));
				player.Attribute[Attributes.charm] += 10;
				player.museum[(int)ExhibitID] = 7;

				ReadRawText(ExhibitInfo.Text[3]);

			}
		}
	}
}
