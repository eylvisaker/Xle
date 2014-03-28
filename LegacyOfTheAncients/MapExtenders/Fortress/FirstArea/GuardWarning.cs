using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.MapExtenders.Fortress.FirstArea
{
	class GuardWarning : EventExtender
	{
		public override void StepOn(GameState state, ref bool handled)
		{
			handled = true;
			TheEvent.Enabled = false;

			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("The guards eye you warily", XleColor.Yellow);

			SoundMan.PlaySoundSync(LotaSound.VeryBad);
		}
	}
}
