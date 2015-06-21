using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ERY.Xle.Services.Implementation;
using ERY.Xle.Services;

namespace ERY.Xle.XleEventTypes.Stores.Extenders
{
    public class StoreBank : StoreFront
    {
        public override int RobValue()
        {
            return Random.Next(180, 231);
        }

        public override bool AllowRobWhenNotAngry
        {
            get
            {
                return true;
            }
        }

        public override bool AllowInteractionWhenLoanOverdue { get { return true; } }

        protected override void InitializeColorScheme(ColorScheme cs)
        {
            cs.BackColor = XleColor.DarkGray;
            cs.FrameColor = XleColor.Green;
            cs.FrameHighlightColor = XleColor.Yellow;
            cs.TitleColor = XleColor.Yellow;
            cs.BorderColor = XleColor.Gray;
        }

        protected override bool SpeakImpl(GameState state)
        {
            int choice;

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

            TextArea.PrintLine();
            TextArea.PrintLine();
            TextArea.PrintLine("Make choice (Hit 0 to cancel)");
            TextArea.PrintLine();

            MenuItemList theList = new MenuItemList("0", "1", "2", "3");
            choice = QuickMenuService.QuickMenu(theList, 2, 0);

            switch (choice)
            {
                case 1:
                    MakeDeposit();
                    break;

                case 2:
                    MakeWithdrawal();
                    break;

                case 3:
                    PrintBalance();
                    break;
            }


            return true;
        }

        private void PrintBalance()
        {
            TextArea.PrintLine("Current balance: " + Player.GoldInBank + " gold.");
        }

        private void MakeWithdrawal()
        {
            if (Player.GoldInBank > 0)
            {
                TextArea.PrintLine();
                TextArea.PrintLine("Withdraw how much?");
                int amount = ChooseNumber(Player.GoldInBank);

                Player.Gold += amount;
                Player.GoldInBank -= amount;
            }
            else
            {
                TextArea.Clear();
                TextArea.PrintLine("Nothing to withdraw");

                StoreSound(LotaSound.Medium);
            }

            TextArea.PrintLine();
            PrintBalance();

            StoreSound(LotaSound.Sale);
        }

        private void MakeDeposit()
        {
            TextArea.PrintLine();
            TextArea.PrintLine("Deposit how much?");
            int amount = ChooseNumber(Player.Gold);

            Player.Spend(amount);
            Player.GoldInBank += amount;

            TextArea.PrintLine();
            PrintBalance();

            StoreSound(LotaSound.Sale);
        }
    }
}
