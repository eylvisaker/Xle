using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes.Stores.Extenders
{
	public class StoreLending : StoreFront
	{
		public override bool AllowInteractionWhenLoanOverdue { get { return true; } }

		public override int RobValue()
		{
			return XleCore.random.Next(180, 231);
		}
		protected override void InitializeColorScheme(ColorScheme cs)
		{
			cs.BackColor = XleColor.DarkGray;
			cs.FrameColor = XleColor.Gray;
			cs.FrameHighlightColor = XleColor.Yellow;
			cs.BorderColor = XleColor.Black;
		}

		protected override bool SpeakImpl(GameState state)
		{
			var player = state.Player;
			int i = 0;
			int max = 200 * player.Level;
			int choice;

			this.player = player;
			robbing = false;

			ClearWindow();

			Title = "Friendly";

			var window1 = new TextWindow { Location = new Point(10, 2), Text = "Lending Association" };

			Windows.Add(window1);

			XleCore.TextArea.PrintLine();

			if (player.loan == 0)
			{
				var window2 = new TextWindow { Location = new Point(8, 7) };

				window2.WriteLine("We'd be happy to loan you");
				window2.WriteLine("money at 'friendly' rates");

				var window3 = new TextWindow { Location = new Point(7, 11) };

				window3.Write("You may borrow up to ");
				window3.Write(max.ToString());
				window3.WriteLine(" gold");

				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine("Borrow how much?");

				Windows.Add(window2);
				Windows.Add(window3);

				choice = ChooseNumber(max);

				if (choice > 0)
				{
					player.Gold += choice;
					player.loan = (int)(choice * 1.5);
					player.dueDate = (int)(player.TimeDays + 0.999) + 120;

					XleCore.TextArea.PrintLine();
					XleCore.TextArea.PrintLine(choice.ToString() + " gold borrowed.");

					XleCore.Wait(1000, DrawStore);

					XleCore.TextArea.Print("You'll owe ", XleColor.White);
					XleCore.TextArea.Print(player.loan.ToString(), XleColor.Yellow);
					XleCore.TextArea.Print(" gold", XleColor.Yellow);
					XleCore.TextArea.Print(" in 120 days.", XleColor.White);
					XleCore.TextArea.PrintLine();

					StoreSound(LotaSound.Bad);
				}
			}
			else
			{
				String DueDate;
				max = Math.Max(player.Gold, player.loan);
				int min;
				int timeLeft = (int)(player.dueDate - player.TimeDays + 0.02);

				if (timeLeft > 0)
				{
					DueDate = timeLeft.ToString() + " days ";
					min = 0;
				}
				else
				{
					DueDate = "NOW!!";
					min = (int)(player.loan * .3 + 0.5);
					if (min > player.Gold)
					{
						min = player.Gold;
						if (player.Gold > 30)
							min -= 10;
					}
				}

				var window2 = new TextWindow { Location = new Point(11, 7) };

				window2.WriteLine("You owe:  " + player.loan.ToString() + " gold!");
				window2.WriteLine();
				window2.WriteLine();
				window2.WriteLine("Due Date: " + DueDate);

				Windows.Add(window2);

				XleCore.TextArea.PrintLine();
				XleCore.TextArea.Print("Pay how much? ");

				if (min > 0)
				{
					XleCore.TextArea.Print("(At Least " + min.ToString() + " gold)", XleColor.Yellow);
				}

				XleCore.TextArea.PrintLine();

				choice = ChooseNumber(max);

				if (choice > player.loan)
					choice = player.loan;

				player.Spend(choice);
				player.loan -= choice;

				if (player.loan <= 0)
				{
					XleCore.TextArea.PrintLine("Loan Repaid.");

					StoreSound(LotaSound.Sale);
				}
				else if (min == 0)
				{
					XleCore.TextArea.PrintLine("You Owe " + player.loan.ToString() + " gold.");

					if (timeLeft > 15)
						XleCore.TextArea.PrintLine("Take your time.");

					StoreSound(LotaSound.Sale);
				}
				else if (choice >= min)
				{
					XleCore.TextArea.PrintLine("You have 14 days to pay the rest!");
					player.dueDate = (int)player.TimeDays + 14;

					StoreSound(LotaSound.Sale);
				}
				else
				{
					XleCore.TextArea.PrintLine("Better pay up!");
					StoreSound(LotaSound.Bad);
				}


			}

			return true;

		}

		
	}
}
