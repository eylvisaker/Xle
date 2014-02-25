using ERY.Xle.XleEventTypes;
using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.MapExtenders.Castle
{
	class PasswordDoor : CastleDoor
	{
		public override void ItemUnlocksDoor(GameState state, int item, ref bool itemUnlocksDoor)
		{
			itemUnlocksDoor = false;
		}

		public override void Speak(GameState state, ref bool handled)
		{
			if (state.Story().HasGuardianPassword)
			{
				g.UpdateBottom("enter command: speak password.");
				SoundMan.PlaySoundSync(LotaSound.VeryGood);

				((Door)TheEvent).RemoveDoor(state);
				handled = true;

				XleCore.TextArea.PrintLine();
			}
		}
	}
}
