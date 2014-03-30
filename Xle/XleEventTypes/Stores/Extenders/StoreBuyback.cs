using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes.Stores.Extenders
{
	public class StoreBuyback : StoreFront
	{
		public override bool AllowInteractionWhenLoanOverdue { get { return true; } }

		protected override void SetColorScheme(ColorScheme cs)
		{
			cs.BackColor = XleColor.Pink;
			cs.FrameColor = XleColor.Yellow;
			cs.FrameHighlightColor = XleColor.Yellow;
			cs.TextAreaBackColor = XleColor.Brown;
			cs.BorderColor = XleColor.Red;
		}
		protected override bool SpeakImpl(GameState state)
		{
			var player = state.Player;

			int i = 0;
			int choice;
			int amount;

			this.player = player;
			robbing = false;

			ClearWindow();
			Title = TheEvent.ShopName;

			var wind = new TextWindow();
			wind.Location = new AgateLib.Geometry.Point(9, 4);
			Windows.Add(wind);

			wind.WriteLine("I will happily purchase");
			wind.WriteLine("your used arms and armor");
			wind.WriteLine();
			wind.WriteLine();
			wind.WriteLine();
			wind.WriteLine("Choose items to sell:");
			wind.WriteLine();
			wind.WriteLine(" 1.  Weapons");
			wind.WriteLine(" 2.  Armor");

			wind.SetColor(XleColor.Red);

			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("Select (0 to cancel)");
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
						XleCore.TextArea.Clear();
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
