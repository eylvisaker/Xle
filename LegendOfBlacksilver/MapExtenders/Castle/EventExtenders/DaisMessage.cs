using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Castle.EventExtenders
{
	class DaisMessage : NullEventExtender
	{
		bool givenMessage = false;
		public override void BeforeStepOn(GameState state)
		{
			if (givenMessage)
				return;
			if (state.Player.Items[LobItem.FalconFeather] == 0)
				return;

			givenMessage = true;

			g.ClearBottom();
			g.UpdateBottom("   You see the prince on a dais!", 3);

			Stopwatch watch = new Stopwatch();
			watch.Start();

			SoundMan.PlaySound(LotaSound.VeryGood);

			while (SoundMan.IsPlaying(LotaSound.VeryGood))
			{
				XleCore.Redraw();

				int index = (int)watch.ElapsedMilliseconds % 100 / 50;

				if (index == 0)
				{
					g.UpdateBottom(g.Bottom(3), 3, XleColor.Yellow);
				}
				else
				{
					g.UpdateBottom(g.Bottom(3), 3, XleColor.Cyan);
				}
			}

		}
	}
}
