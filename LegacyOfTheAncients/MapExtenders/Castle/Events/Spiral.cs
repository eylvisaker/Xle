using System.Collections.Generic;
using System.Linq;

using ERY.Xle.XleEventTypes;
using ERY.Xle.XleEventTypes.Extenders;

namespace ERY.Xle.LotA.MapExtenders.Castle.Events
{
	public class Spiral : EventExtender
	{
		public override bool StepOn(GameState state)
		{
			if (AnyBad(state))
			{
				SoundMan.PlaySoundSync(LotaSound.VeryBad);

				ClearSpiral(state);
				RemoveSpiralEvents(state);

				return true;
			}
			else
				return false;
		}

		protected void RemoveSpiralEvents(GameState state)
		{
			foreach (XleEvent spiralEvent in SpiralEvents(state))
				spiralEvent.Enabled = false;
		}

		protected IEnumerable<XleEvent> SpiralEvents(GameState state)
		{
			return (state.Map.Events.Where(x => x.ExtenderName.ToLowerInvariant().Contains("spiral"))).ToArray();
		}
		protected void ClearSpiral(GameState state)
		{
			foreach (var evt in SpiralEvents(state))
			{
				for (int y = evt.Rectangle.Y; y < evt.Rectangle.Bottom; y++)
				{
					for (int x = evt.Rectangle.X; x < evt.Rectangle.Right; x++)
					{
						state.Map[x, y] = 2;
					}
				}
			}
		}

		protected static bool AnyBad(GameState state)
		{
			int[] vals = new int[4];
			vals[0] = state.Map[state.Player.X, state.Player.Y];
			vals[1] = state.Map[state.Player.X + 1, state.Player.Y];
			vals[2] = state.Map[state.Player.X, state.Player.Y + 1];
			vals[3] = state.Map[state.Player.X + 1, state.Player.Y + 1];

			bool anyBad = vals.Any(x => x > 68);
			return anyBad;
		}
	}

	public class SpiralSuccess : Spiral
	{
		public override bool StepOn(GameState state)
		{
			if (AnyBad(state))
				return false;

			SoundMan.PlaySoundSync(LotaSound.VeryGood);

			ClearSpiral(state);
			RemoveSpiralEvents(state);

			for (int y = 3; y < TheEvent.Rectangle.Y - 1; y++)
			{
				for(int x = TheEvent.Rectangle.X - 3; x <= TheEvent.Rectangle.X; x++)
				{
					state.Map[x, y] = 0;
				}
			}

			return true;
		}
	}
}
