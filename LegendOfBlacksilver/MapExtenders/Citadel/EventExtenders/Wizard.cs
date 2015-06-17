using ERY.Xle.Services.Implementation;
using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Citadel.EventExtenders
{
	class Wizard : EventExtender
	{
		public override bool Speak(GameState state)
		{
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine();

			if (Lob.Story.BoughtOrb == false)
				OfferOrbForSale(state);
			else if (state.Player.Level >= 3 && Lob.Story.BoughtStrengthFromWizard)
				OfferStrengthForSale(state);
			else if (Lob.Story.BoughtStrengthFromWizard == false)
				ComeBackLater(state);
			else
				NoMoreHelp(state);

			XleCore.Wait(state.GameSpeed.AfterSpeakTime);
			return true;
		}

		private void ComeBackLater(GameState state)
		{
			XleCore.TextArea.PrintLine("Come back later.");
		}

		private void OfferOrbForSale(GameState state)
		{
			XleCore.TextArea.PrintLineSlow("I have a certain orb for sale.");
			XleCore.TextArea.PrintLineSlow("The price is only 500 gold, but it");
			XleCore.TextArea.PrintLineSlow("is not cheap.");

			int choice = XleCore.QuickMenuYesNo();

			if (choice == 1)
				return;

			if (state.Player.Gold < 500)
			{
				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine("You don't have enough gold.");
				return;
			}

			state.Player.Gold -= 500;
			state.Player.Items[LobItem.GlassOrb] += 1;
			Lob.Story.BoughtOrb = true;

			SoundMan.PlaySound(LotaSound.VeryBad);

			for(int i = 0; i < 15; i++)
			{
				state.Player.Attribute[Attributes.strength]--;
				XleCore.TextArea.PrintLine(string.Format(
					"Your strength is now: {0}", state.Player.Attribute[Attributes.strength]));

				XleCore.Wait(250);
			}
		}

		private void OfferStrengthForSale(GameState state)
		{
			XleCore.TextArea.PrintLineSlow("I believe our last dealings");
			XleCore.TextArea.PrintLineSlow("cost you some strength.");
			XleCore.TextArea.PrintLineSlow("I can sell you some back for");
			XleCore.TextArea.PrintLineSlow("1,500 gold.");

			int choice = XleCore.QuickMenuYesNo();

			if (choice == 1)
				return;

			if (state.Player.Gold < 1500)
			{
				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine("You don't have enough gold.");
				return;
			}

			state.Player.Attribute[Attributes.strength] += 5;

			Lob.Story.BoughtStrengthFromWizard = true;
		}

		private void NoMoreHelp(GameState state)
		{
			XleCore.TextArea.PrintLine("We have no more dealings.");
		}
	}
}
