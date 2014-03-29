using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders
{
	class ChangeMapTeleporter : ChangeMapExtender
	{
		protected override bool OnStepOnImpl(GameState state, ref bool cancel)
		{
			TeleportAnimation();
			return true;
		}

		protected void TeleportAnimation()
		{
			SoundMan.PlaySound(LotaSound.Teleporter);

			Stopwatch watch = new Stopwatch();
			watch.Start();

			while (watch.ElapsedMilliseconds < 100)
				XleCore.Redraw();

			while (watch.ElapsedMilliseconds < 1800)
			{
				int index = ((int)watch.ElapsedMilliseconds % 80) / 50;

				if (index == 0)
					XleCore.Renderer.PlayerColor = XleColor.Black;
				else
					XleCore.Renderer.PlayerColor = XleColor.White;

				XleCore.Redraw();
			}

			XleCore.Renderer.PlayerColor = XleColor.White;

			while (watch.ElapsedMilliseconds < 2000)
			{
				XleCore.Redraw();
			}
		}
	}
}
