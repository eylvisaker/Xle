using Xle.Maps;
using Xle.Services;
using Xle.Services.MapLoad;
using Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xle.LoB.MapExtenders.Citadel.EventExtenders
{
	public class Mantrek : LobEvent
	{
        public IMapChanger MapChanger { get; set; }

		public override bool Speak()
		{
			if (Story.MantrekKilled)
				return false;

			BegForLife();
			return true;
		}

		private void BegForLife()
		{
			TextArea.PrintLine();
			TextArea.PrintLine();
			TextArea.PrintLine("Spare me and I shall");
			TextArea.PrintLine("give you the staff.");
			TextArea.PrintLine();
			QuickMenu.QuickMenuYesNo();

            MapChanger.ChangeMap(Player.MapID, 1);

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
