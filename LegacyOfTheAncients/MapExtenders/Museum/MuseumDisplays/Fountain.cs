using ERY.Xle.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.MapExtenders.Museum.MuseumDisplays
{
	class Fountain : LotaExhibit
	{
		public Fountain() : base("A Fountain", Coin.Jade) { }
		public override ExhibitIdentifier ExhibitIdentifier { get { return ExhibitIdentifier.Fountain; } }
		public override string LongName
		{
			get
			{
				return "Enchanted flower fountain";
			}
		}

		public override bool IsClosed(ERY.Xle.Player player)
		{
			if (player.Story().Museum[(int)ExhibitIdentifier] == 7)
				return true;
			else
				return false;
		}

		public override void RunExhibit(Player player)
		{
			if (player.Item(10) == 0)
			{
				base.RunExhibit(player);
				g.AddBottom();

				if (player.Story().Museum[(int)ExhibitIdentifier] == 0 || player.Story().Museum[(int)ExhibitIdentifier] == 1)
					g.AddBottom("Do you want to help search?");
				else
					g.AddBottom("Do you want to continue searching?");


				g.AddBottom();
				if (XleCore.QuickMenuYesNo() == 0)
				{
					ReadRawText(ExhibitInfo.Text[2]);
					int amount = player.Story().Museum[3] != 0 ? 300 : 100;
					
					player.Gold += amount;

					//g.UpdateBottom("            Gold:  + " + amount.ToString(), XleColor.Yellow);
					XleCore.TextArea.PrintLine();
					XleCore.TextArea.PrintLine("            Gold:  + " + amount.ToString(), XleColor.Yellow);

					SoundMan.PlaySound(LotaSound.VeryGood);
					XleCore.FlashHPWhileSound(XleColor.Yellow);

					XleCore.WaitForKey();

					player.Story().Museum[(int)ExhibitIdentifier] = 3;
				}
				else
				{
					player.Story().Museum[(int)ExhibitIdentifier] = 1;
				}
			}
			else
			{
				// remove the tulip from the player, give the reward and shut down the exhibit.
				player.ItemCount(10, -player.Item(10));
				player.Attribute[Attributes.charm] += 10;
				player.Story().Museum[(int)ExhibitIdentifier] = 7;

				ReadRawText(ExhibitInfo.Text[3]);

			}
		}
	}
}
