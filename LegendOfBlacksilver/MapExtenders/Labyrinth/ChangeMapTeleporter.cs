using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Labyrinth
{
	class ChangeMapTeleporter : ChangeMapExtender
	{
		public override void OnStepOn(GameState state, ref bool cancel)
		{
			SoundMan.PlaySound(LotaSound.Teleporter);

			Stopwatch watch = new Stopwatch();
			watch.Start();

			while(watch.ElapsedMilliseconds < 1800)
			{
				int index = ((int)watch.ElapsedMilliseconds % 80) / 50;

				if (index == 0)
					XleCore.PlayerColor = XleColor.Black;
				else
					XleCore.PlayerColor = XleColor.White;

				XleCore.Redraw();
			}

			XleCore.PlayerColor = XleColor.White;

			while (watch.ElapsedMilliseconds < 2000)
			{
				XleCore.Redraw();
			}
		}
	}
}
