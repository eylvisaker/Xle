using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Castle.EventExtenders
{
	class Arman : NullEventExtender
	{
		public override void Speak(GameState state, ref bool handled)
		{
			handled = true;
			var ta = XleCore.TextArea;

			ta.PrintLine();

			if (state.Story().DefeatedOrcs == false && state.Story().ArmanGaveElixirs == false)
			{
				state.Story().ArmanGaveElixirs = true;
				state.Player.Items[LobItem.LifeElixir] += 2;

				SoundMan.PlaySound(LotaSound.Good);

				ta.PrintLineSlow("Welcome traveler, I'm arman, the");
				ta.PrintLineSlow("apprentice to the great wizard");
				ta.PrintLineSlow("Seravol.  He's up to his neck with");
				ta.PrintLineSlow("these orcs, farther along.");
				ta.PrintLineSlow();

				XleCore.WaitForKey();

				ta.PrintLineSlow("I know no attack spells, so i'd be");
				ta.PrintLineSlow("most thankful if you could wipe out");
				ta.PrintLineSlow("these orcs.  I also suggest that you");
				ta.PrintLineSlow("gather all the orc loot you can.");
				ta.PrintLineSlow();

				XleCore.WaitForKey();

				SoundMan.PlaySound(LotaSound.VeryGood);
				ta.Clear(true);
				ta.PrintLine();
				ta.PrintLine();
				ta.PrintLineSlow("I can reward you with life elixir.");
				ta.FlashLinesWhile(() => SoundMan.IsPlaying(LotaSound.VeryGood), XleColor.White, XleColor.Yellow, 125);

				XleCore.WaitForKey();

				ta.PrintLine();
				ta.PrintLine();
				ta.PrintLineSlow("In case you missed it, there's a ");
				ta.PrintLineSlow("chest with an archive gem in it.");
				ta.PrintLineSlow("it's between here and the rockslide.");
				ta.PrintLineSlow();
			}
			else
			{
				ta.PrintLine("I'm busy.  Go away now.");
				ta.PrintLine();
			}

			XleCore.WaitForKey();
		}
	}
}
