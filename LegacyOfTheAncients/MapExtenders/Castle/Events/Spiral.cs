﻿using ERY.Xle.XleEventTypes.Extenders;
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
			if (AnyBad(state))
			{
				SoundMan.PlaySoundSync(LotaSound.VeryBad);

				ClearSpiral(state);
				RemoveSpiralEvents(state);

				handled = true;
			}

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
		public override void StepOn(GameState state, ref bool handled)
		{
			if (AnyBad(state))
				return;

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
		}
	}
}