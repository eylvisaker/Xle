using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Citadel.EventExtenders
{
	class Elf : EventExtender
	{
		public override bool Speak(GameState state)
		{
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine();

			if (Lob.Story.ElfPaid == false)
			{
				OfferMoney(state);
				return true;
			}

			if (Lob.Story.ElfSolvedPuzzle == false)
			{
				Puzzle(state);
			}
			else
				NoMoreHelp(state);

			return true;
		}

		private void Puzzle(GameState state)
		{
			XleCore.TextArea.PrintLine("I've prepared a test.");
			XleCore.TextArea.PrintLine();

			bool valid = true;

			XleCore.TextArea.PrintLineSlow("Would you rather rescue a beautiful");
			XleCore.TextArea.PrintLineSlow("princess or a elven baby?\n");

			int choice = XleCore.QuickMenu(new MenuItemList("Princess", "Baby"), 2);
			valid &= (choice == 1);

			XleCore.TextArea.PrintLineSlow("Would you rather slay a marauding");
			XleCore.TextArea.PrintLineSlow("dragon or an incompetent baron?\n");

			choice = XleCore.QuickMenu(new MenuItemList("Dragon", "Baron"), 2);
			valid &= (choice == 1);

			XleCore.TextArea.PrintLineSlow("Would you rather give money to");
			XleCore.TextArea.PrintLineSlow("a hungry thief or a thirsty drunk?\n");

			choice = XleCore.QuickMenu(new MenuItemList("Thief", "Drunk"), 2);
			valid &= (choice == 0);

			if (valid == false)
			{
				XleCore.TextArea.PrintLineSlow("I'm sorry, you suck.");
				return;
			}

			SoundMan.PlaySound(LotaSound.VeryGood);
			XleCore.TextArea.Clear();
			XleCore.TextArea.PrintLineSlow("You have passed the test!\n");
			XleCore.TextArea.PrintLineSlow("A princess has many saviors, \na baby has none.\n");
			XleCore.TextArea.PrintLineSlow("The foibles of a dragon hurt few, \nunlike the failures of a baron.\n");
			XleCore.TextArea.PrintLineSlow("A hungry thief has stopped stealing,\nunlike a thirsty drunk.\n");

			XleCore.TextArea.PrintLineSlow("I have happened across this signet ring. I ");
			XleCore.TextArea.PrintLineSlow("believe it will help you.\n");

			state.Player.Items[LobItem.SignetRing]++;

			Lob.Story.ElfSolvedPuzzle = true;
		}

		private void NoMoreHelp(GameState state)
		{
			XleCore.TextArea.PrintLine("I can't help you anymore.");
		}

		private void OfferMoney(GameState state)
		{
			XleCore.TextArea.PrintLine("Would you rather take 500 gold from");
			XleCore.TextArea.PrintLine("me, or give me 1,500 gold?\n");

			int choice = XleCore.QuickMenu(new MenuItemList("Take", "Give"), 2);

			if (choice == 0)
				state.Player.Gold += 500;
			else if (state.Player.Gold < 1500)
			{
				XleCore.TextArea.PrintLine("You don't have enough gold.");
			}
			else 
			{
				state.Player.Gold -= 1500;
				Lob.Story.ElfPaid = true;

				Puzzle(state);
			}
		}
	}
}
