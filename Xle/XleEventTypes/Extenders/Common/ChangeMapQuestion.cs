using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes.Extenders.Common
{
	public class ChangeMapQuestion : ChangeMapExtender
	{
		ChangeMapEvent ChangeMap { get { return (ChangeMapEvent)TheEvent; } }

		public override void OnStepOn(GameState state, ref bool cancel)
		{
			string newMapName = ChangeMap.GetMapName();
			g.AddBottom("");
			g.AddBottom("Enter " + newMapName + "?");

			SoundMan.PlaySound(LotaSound.Question);

			int choice = XleCore.QuickMenu(new MenuItemList("Yes", "No"), 3);

			if (string.IsNullOrEmpty(ChangeMap.CommandText) == false)
			{
				g.AddBottom("");
				g.AddBottom(string.Format(ChangeMap.CommandText, XleCore.Map.MapName, newMapName));

				g.AddBottom("");
				XleCore.Wait(500);

				choice = 0;
			}

			if (choice == 1)
				cancel = true;
		}
	}
}
