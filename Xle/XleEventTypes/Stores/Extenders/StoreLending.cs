using AgateLib;
using Microsoft.Xna.Framework;
using System;
using System.Threading.Tasks;

namespace Xle.XleEventTypes.Stores.Extenders
{
    [Transient("StoreLending")]
    public class StoreLending : StoreExtender
    {
        public ILendingPresentation LendingPresentation { get; set; }

        public override bool AllowInteractionWhenLoanOverdue { get { return true; } }

        public override int RobValue()
        {
            return Random.Next(180, 231);
        }

        protected override async Task<bool> SpeakImplAsync()
        {
            LendingPresentation.InitializeWindow();

            await TextArea.PrintLine();

            if (Player.loan == 0)
            {
                await OfferLoan();
            }
            else
            {
                await RepayLoan();
            }

            return true;
        }

        private async Task RepayLoan()
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

            var paymentAmount = await LendingPresentation.PaymentPrompt(Player.loan, timeLeft, minPayment, maxPayment);

            if (paymentAmount > Player.loan)
            {
                paymentAmount = Player.loan;
            }

            Player.Gold -= paymentAmount;
            Player.loan -= paymentAmount;

            if (Player.loan <= 0)
            {
                await LendingPresentation.DisplayLoanRepaid();
            }
            else if (minPayment == 0)
            {
                await LendingPresentation.DisplayDebtRemainder(Player.loan, timeLeft);
            }
            else if (paymentAmount >= minPayment)
            {
                Player.dueDate = (int)Player.TimeDays + 14;
                await LendingPresentation.DisplayLoanExtension();
            }
            else
            {
                await LendingPresentation.DisplayFailureToPay();
            }
        }


        public async Task OfferLoan()
        {
            var amount = await LendingPresentation.PromptBorrow(MaxLoan);

            if (amount <= 0)
                return;

            Player.Gold += amount;
            Player.loan = (int)(amount * 1.5);
            Player.dueDate = (int)(Player.TimeDays + 0.999) + 120;

            await LendingPresentation.DisplayNewLoan(amount, Player.loan, 120);
        }

        private int MaxLoan => 200 * Player.Level;
    }

    public interface ILendingPresentation
    {
        void InitializeWindow();

        Task<int> PromptBorrow(int maxLoan);

        Task DisplayNewLoan(int amount, int loan, int i);

        Task DisplayLoanRepaid();

        Task DisplayDebtRemainder(int loan, int timeLeft);

        Task DisplayLoanExtension();

        Task DisplayFailureToPay();

        Task<int> PaymentPrompt(int loan, int timeLeft, int minPayment, int maxPayment);
    }

    [Transient]
    public class LendingPresentation : StoreFront, ILendingPresentation
    {
        public void InitializeWindow()
        {
            ClearWindow();

            Title = "Friendly";

            var window1 = new TextWindow { Location = new Point(10, 2), Text = "Lending Association" };

            Windows.Add(window1);
        }

        public async Task DisplayFailureToPay()
        {
            await TextArea.PrintLine("Better pay up!");
            await StoreSound(LotaSound.Bad);
        }

        public async Task DisplayLoanExtension()
        {
            await TextArea.PrintLine("You have 14 days to pay the rest!");
            await StoreSound(LotaSound.Sale);
        }

        public async Task DisplayDebtRemainder(int loan, int timeLeft)
        {
            await TextArea.PrintLine("You Owe " + loan + " gold.");

            if (timeLeft > 15)
            {
                await TextArea.PrintLine("Take your time.");
            }

            await StoreSound(LotaSound.Sale);
        }


        public async Task DisplayLoanRepaid()
        {
            await TextArea.PrintLine("Loan Repaid.");
            await StoreSound(LotaSound.Sale);
        }

        public async Task<int> PaymentPrompt(int debt, int timeLeft, int minPayment, int maxPayment)
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

            await TextArea.PrintLine();
            await TextArea.Print("Pay how much? ");

            if (minPayment > 0)
            {
                await TextArea.Print("(At Least " + minPayment + " gold)", XleColor.Yellow);
            }

            await TextArea.PrintLine();

            return await ChooseNumber(maxPayment);
        }

        public async Task DisplayNewLoan(int amount, int repaymentAmount, int timeDays)
        {
            await TextArea.PrintLine();
            await TextArea.PrintLine(amount + " gold borrowed.");

            await Wait(1000);

            await TextArea.Print("You'll owe ", XleColor.White);
            await TextArea.Print(repaymentAmount.ToString(), XleColor.Yellow);
            await TextArea.Print(" gold", XleColor.Yellow);
            await TextArea.Print(" in " + timeDays + " days.", XleColor.White);
            await TextArea.PrintLine();

            await StoreSound(LotaSound.Bad);
        }

        public async Task<int> PromptBorrow(int MaxLoan)
        {
            var window2 = new TextWindow { Location = new Point(8, 7) };

            window2.WriteLine("We'd be happy to loan you");
            window2.WriteLine("money at 'friendly' rates");

            var window3 = new TextWindow { Location = new Point(7, 11) };

            window3.Write("You may borrow up to ");
            window3.Write(MaxLoan.ToString());
            window3.WriteLine(" gold");

            await TextArea.PrintLine();
            await TextArea.PrintLine("Borrow how much?");

            Windows.Add(window2);
            Windows.Add(window3);

            return await ChooseNumber(MaxLoan);
        }

        protected override void InitializeColorScheme(ColorScheme cs)
        {
            cs.BackColor = XleColor.DarkGray;
            cs.FrameColor = XleColor.Gray;
            cs.FrameHighlightColor = XleColor.Yellow;
            cs.BorderColor = XleColor.Black;
        }
    }
}
