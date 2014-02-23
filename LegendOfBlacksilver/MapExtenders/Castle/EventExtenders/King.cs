using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Castle.EventExtenders
{
	class King : NullEventExtender
	{
		bool asked;

		public override void Speak(GameState state, ref bool handled)
		{
			g.UpdateBottom("Enter command: speak to the " + Name(state) + ".");
			g.AddBottom("");
			SoundMan.PlaySoundSync(LotaSound.VeryGood);

			if (state.Player.Items[LobItem.FalconFeather] > 0)
			{
				PrinceAskForFeather(state);
			}
			else if (state.Player.Items[LobItem.SmallKey] == 0 && state.Player.Items[LobItem.GoldKey] == 0)
			{
				PrinceAskForHelp(state);
			}

			else 
			{
				NothingToTell();
			}

			XleCore.Wait(2000);

			handled = true;
		}

		private void NothingToTell()
		{
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("     There is no more I can");
			XleCore.TextArea.PrintLine("     tell you right now.  Our");
			XleCore.TextArea.PrintLine("     hopes are still with you.");
			XleCore.TextArea.PrintLine("");

			XleCore.WaitForKey();
		}

		private void PrinceAskForFeather(GameState state)
		{
			asked = false;

			if (asked)
			{
				XleCore.TextArea.PrintLineSlow("I ask again...");

				XleCore.Wait(1000);
				XleCore.TextArea.PrintLineSlow("");
			}
			else
			{
				XleCore.TextArea.Clear();
				XleCore.TextArea.PrintLineSlow("I am the prince of this castle");
				XleCore.TextArea.PrintLineSlow("and all areas around here.");
				XleCore.TextArea.PrintLineSlow();

				XleCore.TextArea.FlashLines(2000, XleColor.Yellow);
				XleCore.Wait(500);
			}

			XleCore.TextArea.PrintLineSlow("I see you have an interesting");
			XleCore.TextArea.PrintLineSlow("feature. May I have it?");

			if (1 == XleCore.QuickMenuYesNo())
			{
				XleCore.TextArea.PrintLineSlow("I've confused you with a brave");
				XleCore.TextArea.PrintLineSlow("warrior who I was expecting.");
				
				asked = true;
			}
			else
			{
				state.Player.Items[LobItem.FalconFeather] = 0;

				XleCore.TextArea.PrintLineSlow("Since you come with the feather, I");
				XleCore.TextArea.PrintLineSlow("know my sister selected you to be our");
				XleCore.TextArea.PrintLineSlow("champion.  I only pray that you are");
				XleCore.TextArea.PrintLineSlow("not too late.  It's been many days");
				XleCore.TextArea.PrintLineSlow("since the king was kidnapped.");

				XleCore.WaitForKey();
				XleCore.TextArea.PrintLineSlow();

				PrinceAskForHelp(state);
			}
		}
		void PrinceAskForHelp(GameState state)
		{
			XleCore.TextArea.PrintLineSlow("There is much to do.  The earthquakes");
			XleCore.TextArea.PrintLineSlow("continue to batter the castle.  A");
			XleCore.TextArea.PrintLineSlow("landslide now blocks entrance to the");
			XleCore.TextArea.PrintLineSlow("inner chambers. The wizard seravol is");
			XleCore.TextArea.PrintLineSlow("trapped inside, imprisoned by orcs.");

			XleCore.WaitForKey();

			XleCore.TextArea.PrintLineSlow();
			XleCore.TextArea.PrintLineSlow("Will you help?");
			XleCore.TextArea.PrintLineSlow();

			if (1 == XleCore.QuickMenuYesNo())
			{
				XleCore.TextArea.PrintLineSlow("I don't blame your fear.");
				XleCore.TextArea.PrintLineSlow("Godspeed on your journeys.");
			}
			else
			{
				XleCore.TextArea.PrintLineSlow("Here is a key.  You must find a way");
				XleCore.TextArea.PrintLineSlow("To get past the landslide.  Once you");
				XleCore.TextArea.PrintLineSlow("do, you'll have to defeat the orcs");
				XleCore.TextArea.PrintLineSlow("who have captured the inner castle");
				XleCore.TextArea.PrintLineSlow("chambers.");

				state.Player.Items[LobItem.SmallKey] = 1;
				XleCore.WaitForKey();

				XleCore.TextArea.PrintLineSlow();
				XleCore.TextArea.PrintLineSlow("My gold is yours - provided you use");
				XleCore.TextArea.PrintLineSlow("it for the good.  The wizard seravol");
				XleCore.TextArea.PrintLineSlow("has much to offer, if you can defeat");
				XleCore.TextArea.PrintLineSlow("the orcs and free him.  Godspeed.");
				XleCore.TextArea.PrintLineSlow();

				XleCore.WaitForKey();
			}
		}

		private string Name(GameState gameState)
		{
			if (gameState.Player.Level > 4)
				return "king";
			else
				return "prince";
		}
	}
}
