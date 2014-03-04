﻿using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Citadel.EventExtenders
{
	class Mantrek : NullEventExtender
	{
		public override void Speak(GameState state, ref bool handled)
		{
			if (state.Story().MantrekKilled)
				return;

			handled = true;
			BegForLife(state);
		}

		private void BegForLife(GameState state)
		{
			XleCore.TextArea.PrintLine("Spare me and I shall");
			XleCore.TextArea.PrintLine("give you the staff.");
			XleCore.TextArea.PrintLine();
			XleCore.QuickMenuYesNo();

			XleCore.ChangeMap(state.Player, state.Player.MapID, 1, 0, 0);

			state.Story().MantrekKilled = true;

			EraseMantrek(state.Map);
		}

		public void EraseMantrek(XleMap map)
		{
			for (int j = 4; j <= 12; j++)
			{
				for (int i = 28; i <= 36; i++)
				{
					if (i < 36)
					{
						map[i, j] = 37;
					}
					else
					{
						map[i, j] = 263;
					}
				}
			}
		}

	}
}