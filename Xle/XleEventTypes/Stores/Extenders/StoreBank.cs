using AgateLib;
using Microsoft.Xna.Framework;
using System;
using System.Threading.Tasks;

namespace Xle.XleEventTypes.Stores.Extenders
{
    [Transient("StoreBank")]
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

        protected override async Task<bool> SpeakImplAsync()
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

            await TextArea.PrintLine();
            await TextArea.PrintLine();
            await TextArea.PrintLine("Make choice (Hit 0 to cancel)");
            await TextArea.PrintLine();

            MenuItemList theList = new MenuItemList("0", "1", "2", "3");

            choice = await QuickMenuService.QuickMenu(theList, 2, 0);

            switch (choice)
            {
                case 1:
                    await MakeDeposit();
                    break;

                case 2:
                    await MakeWithdrawal();
                    break;

                case 3:
                    await PrintBalance();
                    break;
            }


            return true;
        }

        private Task PrintBalance() => TextArea.PrintLine("Current balance: " + Player.GoldInBank + " gold.");

        private async Task MakeWithdrawal()
        {
            if (Player.GoldInBank > 0)
            {
                await TextArea.PrintLine();
                await TextArea.PrintLine("Withdraw how much?");
                int amount = await ChooseNumber(Player.GoldInBank);

                Player.Gold += amount;
                Player.GoldInBank -= amount;
            }
            else
            {
                TextArea.Clear();
                await TextArea.PrintLine("Nothing to withdraw");

                await StoreSound(LotaSound.Medium);
            }

            await TextArea.PrintLine();
            await PrintBalance();

            await StoreSound(LotaSound.Sale);
        }

        private async Task MakeDeposit()
        {
            await TextArea.PrintLine();
            await TextArea.PrintLine("Deposit how much?");
            int amount = await ChooseNumber(Player.Gold);

            Player.Spend(amount);
            Player.GoldInBank += amount;

            await TextArea.PrintLine();
            PrintBalance();

            await StoreSound(LotaSound.Sale);
        }
    }
}
