using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.MapExtenders.Castle
{
	public class Spiral : NullEventExtender
	{
		public override void StepOn(GameState state, ref bool handled)
		{
			int[] vals = new int[4];
			vals[0] = state.Map[state.Player.X, state.Player.Y];
			vals[1] = state.Map[state.Player.X + 1, state.Player.Y];
			vals[2] = state.Map[state.Player.X, state.Player.Y + 1];
			vals[3] = state.Map[state.Player.X + 1, state.Player.Y + 1];

			if (vals.Any(x => x > 68))
			{
				SoundMan.PlaySoundSync(LotaSound.VeryBad);

				for (int y = TheEvent.Rectangle.Y; y < TheEvent.Rectangle.Bottom; y++)
				{
					for (int x = TheEvent.Rectangle.X; x < TheEvent.Rectangle.Right; x++)
					{
						state.Map[x, y] = 2;
					}
				}
				state.Map.Events.Remove(TheEvent);
			}

			handled = true;
		}
	}
}
