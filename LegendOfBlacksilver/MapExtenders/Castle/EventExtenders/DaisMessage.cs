using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Castle.EventExtenders
{
	class DaisMessage : EventExtender
	{
		bool givenMessage = false;
		public override void BeforeStepOn(GameState state)
		{
			if (givenMessage)
				return;
			if (state.Player.Items[LobItem.FalconFeather] == 0)
				return;

			givenMessage = true;

			XleCore.TextArea.Clear(true);
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("   You see the prince on a dais!");

			SoundMan.PlaySound(LotaSound.VeryGood);

			XleCore.TextArea.FlashLinesWhile(() => SoundMan.IsPlaying(LotaSound.VeryGood),
				XleColor.Yellow, XleColor.Cyan, 80, 1);

			XleCore.TextArea.Clear();
		}
	}
}
