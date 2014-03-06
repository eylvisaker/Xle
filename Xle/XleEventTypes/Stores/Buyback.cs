using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes.Stores
{

	public class StoreBuyback : StoreFront
	{
		protected override void SetColorScheme(ColorScheme cs)
		{
			cs.BackColor = XleColor.Pink;
			cs.FrameColor = XleColor.Yellow;
			cs.FrameHighlightColor = XleColor.Yellow;
			cs.TextAreaBackColor = XleColor.Brown;
			cs.BorderColor = XleColor.Red;
		}
		public override bool Speak(GameState state)
		{
			var player = state.Player;

			int i = 0;
			int choice;
			int amount;

			this.player = player;
			robbing = false;

			theWindow[i++] = ShopName;
			theWindow[i++] = "";
			theWindow[i++] = "";
			theWindow[i++] = "I will happily purchase";
			theWindow[i++] = "your used arms and armor";
			theWindow[i++] = "";
			theWindow[i++] = "";
			theWindow[i++] = "";
			theWindow[i++] = "Choose items to sell:";
			theWindow[i++] = "";
			theWindow[i++] = " 1.  Weapons";
			theWindow[i++] = " 2.  Armor";

			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("Select (Hit 0 to cancel)");
			XleCore.TextArea.PrintLine();

			MenuItemList theList = new MenuItemList("0", "1", "2");
			choice = QuickMenu(theList, 2, 0);

			switch (choice)
			{

				case 1:
					XleCore.TextArea.PrintLine();
					XleCore.TextArea.PrintLine("Deposit how much?");
					amount = ChooseNumber(player.Gold);

					player.Spend(amount);
					player.GoldInBank += amount;

					break;
				case 2:
					if (player.GoldInBank > 0)
					{

						XleCore.TextArea.PrintLine();
						XleCore.TextArea.PrintLine("Withdraw how much?");
						amount = ChooseNumber(player.GoldInBank);

						player.Gold += amount;
						player.GoldInBank -= amount;
					}
					else
					{
						g.ClearBottom();
						XleCore.TextArea.PrintLine("Nothing to withdraw");

						StoreSound(LotaSound.Medium);
						choice = 0;

					}
					break;
			}

			if (choice > 0)
			{
				XleCore.TextArea.PrintLine("Current balance: " + player.GoldInBank + " gold.");

				if (choice != 3)
				{
					StoreSound(LotaSound.Sale);
				}

			}

			return true;
		}
	}


}
