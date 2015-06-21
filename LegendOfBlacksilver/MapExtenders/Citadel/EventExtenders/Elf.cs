using ERY.Xle.Services.Implementation;
using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Citadel.EventExtenders
{
    public class Elf : LobEvent
    {
        public override bool Speak(GameState state)
        {
            TextArea.PrintLine();
            TextArea.PrintLine();

            if (Story.ElfPaid == false)
            {
                OfferMoney(state);
                return true;
            }

            if (Story.ElfSolvedPuzzle == false)
            {
                Puzzle(state);
            }
            else
                NoMoreHelp(state);

            return true;
        }

        private void Puzzle(GameState state)
        {
            TextArea.PrintLine("I've prepared a test.");
            TextArea.PrintLine();

            bool valid = true;

            TextArea.PrintLineSlow("Would you rather rescue a beautiful");
            TextArea.PrintLineSlow("princess or a elven baby?\n");

            int choice = QuickMenu.QuickMenu(new MenuItemList("Princess", "Baby"), 2);
            valid &= (choice == 1);

            TextArea.PrintLineSlow("Would you rather slay a marauding");
            TextArea.PrintLineSlow("dragon or an incompetent baron?\n");

            choice = QuickMenu.QuickMenu(new MenuItemList("Dragon", "Baron"), 2);
            valid &= (choice == 1);

            TextArea.PrintLineSlow("Would you rather give money to");
            TextArea.PrintLineSlow("a hungry thief or a thirsty drunk?\n");

            choice = QuickMenu.QuickMenu(new MenuItemList("Thief", "Drunk"), 2);
            valid &= (choice == 0);

            if (valid == false)
            {
                TextArea.PrintLineSlow("I'm sorry, you suck.");
                return;
            }

            SoundMan.PlaySound(LotaSound.VeryGood);
            TextArea.Clear();
            TextArea.PrintLineSlow("You have passed the test!\n");
            TextArea.PrintLineSlow("A princess has many saviors, \na baby has none.\n");
            TextArea.PrintLineSlow("The foibles of a dragon hurt few, \nunlike the failures of a baron.\n");
            TextArea.PrintLineSlow("A hungry thief has stopped stealing,\nunlike a thirsty drunk.\n");

            TextArea.PrintLineSlow("I have happened across this signet ring. I ");
            TextArea.PrintLineSlow("believe it will help you.\n");

            state.Player.Items[LobItem.SignetRing]++;

            Story.ElfSolvedPuzzle = true;
        }

        private void NoMoreHelp(GameState state)
        {
            TextArea.PrintLine("I can't help you anymore.");
        }

        private void OfferMoney(GameState state)
        {
            TextArea.PrintLine("Would you rather take 500 gold from");
            TextArea.PrintLine("me, or give me 1,500 gold?\n");

            int choice = QuickMenu.QuickMenu(new MenuItemList("Take", "Give"), 2);

            if (choice == 0)
                state.Player.Gold += 500;
            else if (state.Player.Gold < 1500)
            {
                TextArea.PrintLine("You don't have enough gold.");
            }
            else
            {
                state.Player.Gold -= 1500;
                Story.ElfPaid = true;

                Puzzle(state);
            }
        }
    }
}
