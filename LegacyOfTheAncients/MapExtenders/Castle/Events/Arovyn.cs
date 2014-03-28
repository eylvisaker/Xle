using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.MapExtenders.Castle
{
	class Arovyn : EventExtender
	{
		public override void Speak(GameState state, ref bool handled)
		{
			if (state.Player.Attribute[Attributes.strength] <= 25)
			{
				TooWeakMessage(state);
			}
			else
			{
				GiveMark(state);
			}

			handled = true;
		}

		private void TooWeakMessage(GameState state)
		{
			var ta = XleCore.TextArea;

			ta.PrintLine();
			ta.PrintLine();
			ta.PrintLineSlow("I would like to confide in you, but", XleColor.Yellow);
			ta.PrintLineSlow("you are not strong enough to help.", XleColor.Yellow);
			ta.PrintLineSlow("see me when your strength has grown.", XleColor.Yellow);

			XleCore.Wait(1500);

		}

		private static void GiveMark(GameState state)
		{
			var ta = XleCore.TextArea;

			ta.PrintLine();
			ta.PrintLine();
			ta.PrintLineSlow("My health declines.  You are my last", XleColor.Yellow);
			ta.PrintSlow("hope.  Find the ", XleColor.Yellow);
			ta.PrintLineSlow("guardians of the", XleColor.White);
			ta.PrintSlow("scroll.  ", XleColor.White);
			ta.PrintLineSlow("They are in many towns, but", XleColor.Yellow);
			ta.PrintLineSlow("talk only to those with a special", XleColor.Yellow);
			ta.PrintLineSlow("secret mark.", XleColor.Yellow);

			XleCore.Wait(3000);

			ta.PrintLineSlow("I've now put this magic mark on your", XleColor.Cyan);
			ta.PrintLineSlow("forearm.  Only guardians can see it.", XleColor.Cyan);

			XleCore.Wait(4000);

			Lota.Story.HasGuardianMark = true;
		}
	}
}
