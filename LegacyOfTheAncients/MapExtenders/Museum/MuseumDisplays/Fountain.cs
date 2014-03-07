using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;
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
			if (Lota.Story.Museum[(int)ExhibitIdentifier] >= 10)
				return true;
			else
				return false;
		}

		public override void RunExhibit(Player player)
		{
			if (player.Items[LotaItem.Tulip] == 0)
			{
				OfferTulipQuest(player);
			}
			else
			{
				RewardForTulip(player);
			}
		}

		private void RewardForTulip(Player player)
		{
			// remove the tulip from the player, give the reward and shut down the exhibit.
			player.Items[LotaItem.Tulip] = 0;
			player.Attribute[Attributes.charm] += 10;
			Lota.Story.Museum[ExhibitID] = 10;

			ReadRawText(ExhibitInfo.Text[3]);
		}

		private void OfferTulipQuest(Player player)
		{
			base.RunExhibit(player);
			XleCore.TextArea.PrintLine();

			if (Lota.Story.Museum[(int)ExhibitIdentifier] == 0 || Lota.Story.Museum[(int)ExhibitIdentifier] == 1)
				XleCore.TextArea.PrintLine("Do you want to help search?");
			else
				XleCore.TextArea.PrintLine("Do you want to continue searching?");


			XleCore.TextArea.PrintLine();
			if (XleCore.QuickMenuYesNo() == 0)
			{
				ReadRawText(ExhibitInfo.Text[2]);
				int amount = Lota.Story.Museum[3] != 0 ? 300 : 100;

				player.Gold += amount;

				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine("            Gold:  + " + amount.ToString(), XleColor.Yellow);

				SoundMan.PlaySound(LotaSound.VeryGood);
				XleCore.FlashHPWhileSound(XleColor.Yellow);

				XleCore.WaitForKey();

				Lota.Story.Museum[(int)ExhibitIdentifier] = 3;
			}
			else
			{
				Lota.Story.Museum[(int)ExhibitIdentifier] = 1;
			}
		}
	}
}
