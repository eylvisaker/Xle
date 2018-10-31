using System.Threading.Tasks;

namespace Xle.Blacksilver.MapExtenders.Citadel.EventExtenders
{
    public class Elf : LobEvent
    {
        public override async Task<bool> Speak()
        {
            await TextArea.PrintLine();
            await TextArea.PrintLine();

            if (Story.ElfPaid == false)
            {
                await OfferMoney();
                return true;
            }

            if (Story.ElfSolvedPuzzle == false)
            {
                await Puzzle();
            }
            else
                await NoMoreHelp();

            return true;
        }

        private async Task Puzzle()
        {
            await TextArea.PrintLine("I've prepared a test.");
            await TextArea.PrintLine();

            bool valid = true;

            await TextArea.PrintLineSlow("Would you rather rescue a beautiful");
            await TextArea.PrintLineSlow("princess or a elven baby?\n");

            int choice = await QuickMenu.QuickMenu(new MenuItemList("Princess", "Baby"), 2);
            valid &= (choice == 1);

            await TextArea.PrintLineSlow("Would you rather slay a marauding");
            await TextArea.PrintLineSlow("dragon or an incompetent baron?\n");

            choice = await QuickMenu.QuickMenu(new MenuItemList("Dragon", "Baron"), 2);
            valid &= (choice == 1);

            await TextArea.PrintLineSlow("Would you rather give money to");
            await TextArea.PrintLineSlow("a hungry thief or a thirsty drunk?\n");

            choice = await QuickMenu.QuickMenu(new MenuItemList("Thief", "Drunk"), 2);
            valid &= (choice == 0);

            if (valid == false)
            {
                await TextArea.PrintLineSlow("I'm sorry, you suck.");
                return;
            }

            SoundMan.PlaySound(LotaSound.VeryGood);
            TextArea.Clear();
            await TextArea.PrintLineSlow("You have passed the test!\n");
            await TextArea.PrintLineSlow("A princess has many saviors, \na baby has none.\n");
            await TextArea.PrintLineSlow("The foibles of a dragon hurt few, \nunlike the failures of a baron.\n");
            await TextArea.PrintLineSlow("A hungry thief has stopped stealing,\nunlike a thirsty drunk.\n");

            await TextArea.PrintLineSlow("I have happened across this signet ring. I ");
            await TextArea.PrintLineSlow("believe it will help you.\n");

            Player.Items[LobItem.SignetRing]++;

            Story.ElfSolvedPuzzle = true;
        }

        private Task NoMoreHelp()
        {
            return TextArea.PrintLine("I can't help you anymore.");
        }

        private async Task OfferMoney()
        {
            await TextArea.PrintLine("Would you rather take 500 gold from");
            await TextArea.PrintLine("me, or give me 1,500 gold?\n");

            int choice = await QuickMenu.QuickMenu(new MenuItemList("Take", "Give"), 2);

            if (choice == 0)
                Player.Gold += 500;
            else if (Player.Gold < 1500)
            {
                await TextArea.PrintLine("You don't have enough gold.");
            }
            else
            {
                Player.Gold -= 1500;
                Story.ElfPaid = true;

                await Puzzle();
            }
        }
    }
}
