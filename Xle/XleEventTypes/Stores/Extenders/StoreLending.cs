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
			return Random.Next(180, 231);
		}

		protected override void InitializeColorScheme(ColorScheme cs)
		{
			cs.BackColor = XleColor.DarkGray;
			cs.FrameColor = XleColor.Gray;
			cs.FrameHighlightColor = XleColor.Yellow;
			cs.BorderColor = XleColor.Black;
		}

		protected override bool SpeakImpl()
		{
			robbing = false;

			InitializeWindow();

			TextArea.PrintLine();

			if (Player.loan == 0)
			{
				OfferLoan();
			}
			else
			{
				RepayLoan();
			}

			return true;
		}

		private void InitializeWindow()
		{
			ClearWindow();

			Title = "Friendly";

			var window1 = new TextWindow { Location = new Point(10, 2), Text = "Lending Association" };

			Windows.Add(window1);
		}

		private void RepayLoan()
		{
			int maxPayment = Math.Max(Player.Gold, Player.loan);
			int minPayment;
			int timeLeft = (int)(Player.dueDate - Player.TimeDays + 0.02);

			if (timeLeft > 0)
			{
				minPayment = 0;
			}
			else
			{
				minPayment = (int)(Player.loan * .3 + 0.5);

				if (minPayment > Player.Gold)
				{
					minPayment = Player.Gold;
					if (Player.Gold > 30)
					{
						minPayment -= 10;
					}
				}
			}

			var paymentAmount = PaymentPrompt(Player.loan, timeLeft, minPayment, maxPayment);

			if (paymentAmount > Player.loan)
			{
				paymentAmount = Player.loan;
			}

			Player.Gold -= paymentAmount;
			Player.loan -= paymentAmount;

			if (Player.loan <= 0)
			{
				DisplayLoanRepaid();
			}
			else if (minPayment == 0)
			{
				DisplayDebtRemainder(Player.loan, timeLeft);
			}
			else if (paymentAmount >= minPayment)
			{
				Player.dueDate = (int)Player.TimeDays + 14;
				DisplayLoanExtension();
			}
			else
			{
				DisplayFailureToPay();
			}
		}

		private void DisplayFailureToPay()
		{
			TextArea.PrintLine("Better pay up!");
			StoreSound(LotaSound.Bad);
		}

		private void DisplayLoanExtension()
		{
			TextArea.PrintLine("You have 14 days to pay the rest!");

			StoreSound(LotaSound.Sale);
		}

		private void DisplayDebtRemainder(int loan, int timeLeft)
		{
			TextArea.PrintLine("You Owe " + loan + " gold.");

			if (timeLeft > 15)
			{
				TextArea.PrintLine("Take your time.");
			}

			StoreSound(LotaSound.Sale);
		}

		private void DisplayLoanRepaid()
		{
			TextArea.PrintLine("Loan Repaid.");

			StoreSound(LotaSound.Sale);
		}

		private int PaymentPrompt(int debt, int timeLeft, int minPayment, int maxPayment)
		{
			string dueDate;

			if (timeLeft > 0)
			{
				dueDate = timeLeft + " days ";
			}
			else
			{
				dueDate = "NOW!!";
			}

			var window2 = new TextWindow { Location = new Point(11, 7) };

			window2.WriteLine("You owe:  " + debt + " gold!");
			window2.WriteLine();
			window2.WriteLine();
			window2.WriteLine("Due Date: " + dueDate);

			Windows.Add(window2);

			TextArea.PrintLine();
			TextArea.Print("Pay how much? ");

			if (minPayment > 0)
			{
				TextArea.Print("(At Least " + minPayment + " gold)", XleColor.Yellow);
			}

			TextArea.PrintLine();

			var choice = ChooseNumber(maxPayment);

			return choice;
		}

		private void OfferLoan()
		{
			var amount = PromptBorrow();

			if (amount <= 0)
				return;

			Player.Gold += amount;
			Player.loan = (int)(amount * 1.5);
			Player.dueDate = (int)(Player.TimeDays + 0.999) + 120;

			DisplayNewLoan(amount, Player.loan, 120);
		}

		private void DisplayNewLoan(int amount, int repaymentAmount, int timeDays)
		{
			TextArea.PrintLine();
			TextArea.PrintLine(amount + " gold borrowed.");

			Wait(1000);

			TextArea.Print("You'll owe ", XleColor.White);
			TextArea.Print(repaymentAmount.ToString(), XleColor.Yellow);
			TextArea.Print(" gold", XleColor.Yellow);
			TextArea.Print(" in " + timeDays + " days.", XleColor.White);
			TextArea.PrintLine();

			StoreSound(LotaSound.Bad);
		}

		private int PromptBorrow()
		{
			var window2 = new TextWindow { Location = new Point(8, 7) };

			window2.WriteLine("We'd be happy to loan you");
			window2.WriteLine("money at 'friendly' rates");

			var window3 = new TextWindow { Location = new Point(7, 11) };

			window3.Write("You may borrow up to ");
			window3.Write(MaxLoan.ToString());
			window3.WriteLine(" gold");

			TextArea.PrintLine();
			TextArea.PrintLine("Borrow how much?");

			Windows.Add(window2);
			Windows.Add(window3);

			return ChooseNumber(MaxLoan);
		}

		private int MaxLoan
		{
			get { return 200 * Player.Level; }
		}
	}
}
