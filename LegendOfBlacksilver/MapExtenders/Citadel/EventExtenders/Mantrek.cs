﻿using ERY.Xle.Maps;
using ERY.Xle.Services;
using ERY.Xle.Services.MapLoad;
using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Citadel.EventExtenders
{
	public class Mantrek : LobEvent
	{
        public IMapChanger MapChanger { get; set; }

		public override bool Speak(GameState state)
		{
			if (Story.MantrekKilled)
				return false;

			BegForLife(state);
			return true;
		}

		private void BegForLife(GameState state)
		{
			TextArea.PrintLine();
			TextArea.PrintLine();
			TextArea.PrintLine("Spare me and I shall");
			TextArea.PrintLine("give you the staff.");
			TextArea.PrintLine();
			QuickMenu.QuickMenuYesNo();

            MapChanger.ChangeMap(Player, Player.MapID, 1);

			Story.MantrekKilled = true;

			EraseMantrek(Map);
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
