﻿using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.MapExtenders.Fortress
{
	class Elevator : NullEventExtender
	{
		public override void StepOn(GameState state, ref bool handled)
		{
			handled = true;

			int ystart = state.Player.Y;

			while (state.Player.X < TheEvent.Rectangle.Right)
			{
				AdvancePlayer(state);

				if (state.Player.X == 25)
				{
					for (int i = 0; i < 2; i++)
					{
						state.Player.X = 11;
						state.Player.Y = 63;

						do
						{
							AdvancePlayer(state);

						} while (state.Player.X != 98);
					}

					state.Player.Y = 34;
					state.Player.X = 25;
				}
			}
		}

		private static void AdvancePlayer(GameState state)
		{
			XleCore.Wait(125);
			state.Player.X++;
		} 
	}
}