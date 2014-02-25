using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.MapExtenders.Castle
{
	class Arovyn : NullEventExtender
	{
		public override void Speak(GameState state, ref bool handled)
		{
			var ta = XleCore.TextArea;

			ta.PrintLine();
			ta.PrintLineSlow("My health declines.  You are my last", XleColor.Yellow);
			ta.PrintSlow("hope.  Find the ", XleColor.Yellow);
			ta.PrintLineSlow("guardians of the", XleColor.White);
			ta.PrintSlow("scroll.  ", XleColor.White);
			ta.PrintLineSlow("They are in many towns, but", XleColor.Yellow);
			ta.PrintLineSlow("talk only to those with a special", XleColor.Yellow);
			ta.PrintLineSlow("secret mark.", XleColor.Yellow);
			
			XleCore.Wait(1500);

			ta.PrintLineSlow("I've now put this magic mark on your", XleColor.Cyan);
			ta.PrintLineSlow("forearm.  Only guardians can see it.", XleColor.Cyan);

			state.Story().HasGuardianMark = true;
		}
	}
}
