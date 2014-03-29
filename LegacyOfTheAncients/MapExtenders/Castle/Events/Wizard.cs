using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.MapExtenders.Castle
{
	class Wizard : EventExtender
	{
		public override bool Speak(GameState state)
		{
			SoundMan.PlaySound(LotaSound.VeryGood);

			XleCore.TextArea.Clear(true);
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("    Meet the wizard of potions!!", XleColor.Cyan);
			XleCore.TextArea.PrintLine();

			XleCore.TextArea.FlashLinesWhile(() => SoundMan.IsPlaying(LotaSound.VeryGood), XleColor.Green, XleColor.Cyan, 250);

			if (Lota.Story.BoughtPotion)
			{
				BegoneMessage();
			}
			else
			{
				OfferPotion(state);
			}

			XleCore.Wait(5000);
			return true;
		}

		private void OfferPotion(GameState state)
		{
			XleCore.TextArea.PrintLine("My potion can help you.");
			XleCore.TextArea.PrintLine("It will cost 2,500 gold.");
			XleCore.TextArea.PrintLine();

			if (XleCore.QuickMenuYesNo() == 0)
			{
				if (state.Player.Gold < 2500)
				{
					XleCore.TextArea.PrintLine();
					XleCore.TextArea.PrintLine("you haven't the gold.");
				}
				else
				{
					state.Player.Gold -= 2500;
					Lota.Story.BoughtPotion = true;

					if (state.Player.Attribute[Attributes.dexterity] <= state.Player.Attribute[Attributes.endurance])
					{
						state.Player.Attribute[Attributes.dexterity] = 36;
						state.Player.Attribute[Attributes.endurance] += 5;
					}
					else
					{
						state.Player.Attribute[Attributes.dexterity] += 5;
						state.Player.Attribute[Attributes.endurance] = 36;
					}

					XleCore.TextArea.Clear(true);
					XleCore.TextArea.PrintLine();
					XleCore.TextArea.PrintLine("Check your attributes.");
					XleCore.TextArea.PrintLine();

					SoundMan.PlaySound(LotaSound.VeryGood);

					XleCore.TextArea.FlashLinesWhile(() => SoundMan.IsPlaying(LotaSound.VeryGood), XleColor.White, XleColor.Cyan, 250);

				}
			}
			else
			{
				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine("No?  Maybe later.");
			}
		}

		private void BegoneMessage()
		{
			XleCore.TextArea.PrintLine("I can do no more for you.");
			XleCore.TextArea.PrintLine();
		}
	}
}
