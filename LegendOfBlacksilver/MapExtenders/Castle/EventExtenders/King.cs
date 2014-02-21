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
		public override void Speak(GameState gameState, ref bool handled)
		{
			g.ClearBottom();
			g.AddBottom("Speak to the prince!");

			SoundMan.PlaySoundSync(LotaSound.VeryGood);

			g.ClearBottom();
			g.WriteSlow("Hi I'm the prince.", 0, XleColor.White);
			g.WriteSlow("I'm taking your feather.", 1, XleColor.White);
			g.WriteSlow("And giving you this key.", 2, XleColor.White);

			gameState.Player.Items[LobItem.FalconFeather] = 0;
			gameState.Player.Items[LobItem.SmallKey] = 1;

			XleCore.wait(2000);
		}
	}
}
