using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Outside.Events
{
	class Drawbridge : ChangeMapExtender
	{
		public override void OnStepOn(GameState state, ref bool cancel)
		{
			XleCore.TextArea.PrintLine();

			if (state.Player.Items[LobItem.RopeAndPulley] == 0)
			{
				XleCore.TextArea.PrintLine("You're not equipped");
				XleCore.TextArea.PrintLine("to storm the citadel.");
				SoundMan.PlaySound(LotaSound.Bad);
			}
			else
			{
				XleCore.TextArea.PrintLine("The drawbridge is up.");
				XleCore.TextArea.PrintLine("You may wish to lower it.");
				SoundMan.PlaySound(LotaSound.Question);
			}

			XleCore.Wait(1000);

			cancel = true;
		}

		public override void Use(GameState state, int item, ref bool handled)
		{
			if (item == (int)LobItem.RopeAndPulley)
			{
				handled = true;

				XleCore.Wait(1000);
				TheEvent.ExecuteMapChange(state.Player);
			}
		}
	}
}
