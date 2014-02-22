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

			XleCore.wait(2000);

			handled = true;
		}

		private void NothingToTell()
		{
			g.AddBottom("");
			g.AddBottom("     There is no more I can");
			g.AddBottom("     tell you right now.  Our");
			g.AddBottom("     hopes are still with you.");
			g.AddBottom("");

			XleCore.WaitForKey();
		}

		private void PrinceAskForFeather(GameState state)
		{
			if (asked)
			{
				g.AddBottom("");
				g.WriteSlow("I ask again...", 0, XleColor.White);
				g.AddBottom("");
				XleCore.wait(1000);
			}

			g.AddBottom("");
			g.WriteSlow("I see you have an interesting ", 0, XleColor.White);
			g.AddBottom("");
			g.WriteSlow("feather. May I have it?", 0, XleColor.White);

			if (1 == XleCore.QuickMenuYesNo())
			{
				g.AddBottom("");
				g.WriteSlow("I've confused you with a brave", 0, XleColor.White);
				g.AddBottom("");
				g.WriteSlow("warrior who I was expecting.", 0, XleColor.White);
				g.AddBottom("");

				asked = true;
			}
			else
			{
				state.Player.Items[LobItem.FalconFeather] = 0;

				g.AddBottom("");
				g.WriteSlow("Since you come with the feather, I", 0, XleColor.White);

				g.AddBottom(""); 
				g.WriteSlow("know my sister selected you to be our", 0, XleColor.White);

				g.AddBottom(""); 
				g.WriteSlow("champion.  I only pray that you are", 0, XleColor.White);

				g.AddBottom(""); 
				g.WriteSlow("not too late.  It's been many days", 0, XleColor.White);

				g.AddBottom(""); 
				g.WriteSlow("since the king was kidnapped.", 0, XleColor.White);

				XleCore.WaitForKey();
				g.AddBottom("");

				PrinceAskForHelp(state);
			}
		}
		void PrinceAskForHelp(GameState state)
		{
			g.AddBottom("");
			g.WriteSlow("There is much to do.  The earthquakes", 0, XleColor.White);

			g.AddBottom("");
			g.WriteSlow("continue to batter the castle.  A", 0, XleColor.White);

			g.AddBottom("");
			g.WriteSlow("landslide now blocks entrance to the", 0, XleColor.White);

			g.AddBottom("");
			g.WriteSlow("inner chambers. The wizard seravol is", 0, XleColor.White);

			g.AddBottom("");
			g.WriteSlow("trapped inside, imprisoned by orcs.", 0, XleColor.White);

			XleCore.WaitForKey();

			g.AddBottom("");
			g.AddBottom("");
			g.WriteSlow("Will you help?", 0, XleColor.White);
			g.AddBottom("");

			if (1 == XleCore.QuickMenuYesNo())
			{
				g.AddBottom("");
				g.WriteSlow("I don't blame your fear.", 0, XleColor.White);
				g.AddBottom("");
				g.WriteSlow("Godspeed on your journeys.", 0, XleColor.White);
			}
			else
			{
				g.AddBottom("");
				g.WriteSlow("Here is a key.  You must find a way", 0, XleColor.White);
				g.AddBottom("");
				g.WriteSlow("To get past the landslide.  Once you", 0, XleColor.White);
				g.AddBottom("");
				g.WriteSlow("do, you'll have to defeat the orcs", 0, XleColor.White);
				g.AddBottom("");
				g.WriteSlow("who have captured the inner castle", 0, XleColor.White);
				g.AddBottom("");
				g.WriteSlow("chambers.", 0, XleColor.White);

				state.Player.Items[LobItem.SmallKey] = 1;
				XleCore.WaitForKey();

				g.AddBottom("");
				g.AddBottom("");
				g.WriteSlow("My gold is yours - provided you use", 0, XleColor.White);
				g.AddBottom("");
				g.WriteSlow("it for the good.  The wizard seravol", 0, XleColor.White);
				g.AddBottom("");
				g.WriteSlow("has much to offer, if you can defeat", 0, XleColor.White);
				g.AddBottom("");
				g.WriteSlow("the orcs and free him.  Godspeed.", 0, XleColor.White);
				g.AddBottom(""); 

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
