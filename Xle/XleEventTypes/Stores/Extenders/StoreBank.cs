using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes.Stores.Extenders
{
	public class StoreBank : StoreFront
	{
		public override int RobValue()
		{
			return XleCore.random.Next(180, 231);
		}

		public override bool AllowRobWhenNotAngry
		{
			get
			{
				return true;
			}
		}
		
		public override bool AllowInteractionWhenLoanOverdue { get { return true; } }

		protected override void SetColorScheme(ColorScheme cs)
		{
			cs.BackColor = XleColor.DarkGray;
			cs.FrameColor = XleColor.Green;
			cs.FrameHighlightColor = XleColor.Yellow;
			cs.TitleColor = XleColor.Yellow;
			cs.BorderColor = XleColor.Gray;
		}

		protected override bool SpeakImpl(GameState state)
		{
			var player = state.Player;

			int choice;

			this.player = player;
			robbing = false;

			ClearWindow();

			Title = "Convenience Bank";

			var promptWindow = new TextWindow();
			promptWindow.Location = new Point(14, 3);
			promptWindow.WriteLine("Our Services");
			promptWindow.WriteLine("---------------");

			var optionsWindow = new TextWindow();
			optionsWindow.Location = new Point(10, 7);

			optionsWindow.WriteLine("1.  Deposit Funds");
			optionsWindow.WriteLine();
			optionsWindow.WriteLine("2.  Withdraw Funds");
			optionsWindow.WriteLine();
			optionsWindow.WriteLine("3.  Balance Inquiry");

			Windows.Add(promptWindow);
			Windows.Add(optionsWindow);

			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("Make choice (Hit 0 to cancel)");
			XleCore.TextArea.PrintLine();

			MenuItemList theList = new MenuItemList("0", "1", "2", "3");
			choice = QuickMenu(theList, 2, 0);

			switch (choice)
			{
				case 1:
					MakeDeposit(player);
					break;

				case 2:
					MakeWithdrawal(player, choice);
					break;

				case 3:
					PrintBalance(player);
					break;
			}

					
			return true;
		}

		private static void PrintBalance(Player player)
		{
			XleCore.TextArea.PrintLine("Current balance: " + player.GoldInBank + " gold.");
		}

		private void MakeWithdrawal(Player player, int choice)
		{
			if (player.GoldInBank > 0)
			{
				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine("Withdraw how much?");
				int amount = ChooseNumber(player.GoldInBank);

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

			XleCore.TextArea.PrintLine();
			PrintBalance(player);

			StoreSound(LotaSound.Sale);
		}

		private void MakeDeposit(Player player)
		{
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("Deposit how much?");
			int amount = ChooseNumber(player.Gold);

			player.Spend(amount);
			player.GoldInBank += amount;

			XleCore.TextArea.PrintLine();
			PrintBalance(player);

			StoreSound(LotaSound.Sale);
		}
	}
}
