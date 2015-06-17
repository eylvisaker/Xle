using ERY.Xle.Services.Implementation;
using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Castle.EventExtenders
{
	class Seravol : EventExtender
	{
		public override bool Speak(GameState state)
		{
			Greetings();
			bool handled = false;

			if (state.Player.Level > 1)
			{
				if (state.Player.Items[LobItem.GoldKey] == 0 &&
					AllOrcsKilled(state))
				{
					GiveGoldKey(state);
					handled = true;
				}
			}
			else
			{
				Introduction(state);
				handled = true;
			}

			if (XleCore.random.NextDouble() < 0.4)
			{
				QuakeMessage();
			}

			if (handled == false)
			{
				NothingToTellMessage(state);
			}

			return true;
		}

		private void NothingToTellMessage(GameState state)
		{
			XleCore.TextArea.PrintLineSlow();
			XleCore.TextArea.PrintLine("I have nothing else to tell you.");
			XleCore.TextArea.PrintLine();
			XleCore.Wait(1000);
			XleCore.TextArea.PrintLine("Perhaps the prince could");
			XleCore.TextArea.PrintLine("Help you further.");
			XleCore.WaitForKey();
		}

		private void GiveGoldKey(GameState state)
		{
			XleCore.TextArea.PrintLineSlow(); 
			XleCore.TextArea.PrintLineSlow("I've taken your small and wooden");
			XleCore.TextArea.PrintLineSlow("keys in exchange for this gold key.");
			XleCore.TextArea.PrintLineSlow("I suspect you'll find the new one");
			XleCore.TextArea.PrintLineSlow("more convenient.");
			XleCore.TextArea.PrintLineSlow();
			XleCore.WaitForKey();

			state.Player.Items[LobItem.GoldKey] = 1;
			state.Player.Items[LobItem.WoodenKey] = 0;
			state.Player.Items[LobItem.SmallKey] = 0;

			Lob.Story.ClearedRockSlide = true;
			Lob.Story.DefeatedOrcs = true;

			XleCore.TextArea.PrintLineSlow("My orb of vision has been stolen and");
			XleCore.TextArea.PrintLineSlow("removed from this castle.  Without ");
			XleCore.TextArea.PrintLineSlow("it, I'm almost blind.  With it, there");
			XleCore.TextArea.PrintLineSlow("is much I could tell you.");
			XleCore.TextArea.PrintLineSlow();
			XleCore.WaitForKey();

			SoundMan.PlaySound(LotaSound.VeryGood);
			XleCore.TextArea.PrintLineSlow("To spur you on, I present this red");
			XleCore.TextArea.PrintLineSlow("garnet gem.  Use it wisely.");
			XleCore.WaitForKey();

			state.Player.Items[LobItem.RedGarnet] += 1;
		}

		private static void QuakeMessage()
		{

			XleCore.TextArea.PrintLineSlow("I've heard that the quakes seem to");
			XleCore.TextArea.PrintLineSlow("be disturbing the tides of bantross.");
			XleCore.WaitForKey();
		}

		private bool AllOrcsKilled(GameState state)
		{
			if (state.Map.Guards.Any(x => x.Color == XleColor.Blue))
				return false;
			else
				return true;
		}

		private void Introduction(GameState state)
		{
			var ta = XleCore.TextArea;

			ta.PrintLineSlow("Ah, it is a pleasure to at last be");
			ta.PrintLineSlow("free from these vile and troublesome");
			ta.PrintLineSlow("orc guards.  One good turn deserves");
			ta.PrintLineSlow("another.");
			ta.PrintLineSlow();

			XleCore.WaitForKey();
			
			ta.Clear(true);
			ta.PrintLine();
			ta.PrintLine();
			ta.PrintLine("I promote you to apprentice.");
			ta.PrintLine();

			SoundMan.PlaySound(LotaSound.VeryGood);
			ta.FlashLinesWhile(() => SoundMan.IsPlaying(LotaSound.VeryGood),
				XleColor.White, XleColor.Red, 250);
			
			ta.PrintLineSlow("(Admittedly, it's not much, but");
			ta.PrintLineSlow("far better than being a serf.)");
			ta.PrintLineSlow();

			SoundMan.PlaySound(LotaSound.Good);
			ta.PrintLineSlow("Charisma: +5");

			state.Player.Attribute[Attributes.charm] += 5;
			state.Player.Level = 2;

			ta.PrintLineSlow();
			XleCore.WaitForKey();

			if (AllOrcsKilled(state))
			{
				GiveGoldKey(state);
			}
			else
			{
				ta.PrintLineSlow("Please get rid of the rest of these");
				ta.PrintLineSlow("orcs.  If you don't, they'll only");
				ta.PrintLineSlow("come back.");
				ta.PrintLineSlow();
			}

			XleCore.WaitForKey();
		}

		private static void Greetings()
		{
			XleCore.TextArea.Clear(true);
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("Greetings from the wizard seravol!");
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine();

			SoundMan.PlaySound(LotaSound.VeryGood);
			XleCore.TextArea.FlashLinesWhile(
				() => SoundMan.IsPlaying(LotaSound.VeryGood),
				XleColor.White, XleColor.Yellow, 250);
		}
	}
}
