using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ERY.Xle.Services.Implementation;

namespace ERY.Xle.XleEventTypes.Extenders.Common
{
	public class ChangeMapQuestion : ChangeMapExtender
	{
		protected override bool OnStepOnImpl(GameState state, ref bool cancel)
		{
			string newMapName = GetMapName();
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("Enter " + newMapName + "?");

			SoundMan.PlaySound(LotaSound.Question);

			int choice = XleCore.QuickMenu(new MenuItemList("Yes", "No"), 3);

			if (choice == 1)
				return false;
			else if (string.IsNullOrEmpty(TheEvent.CommandText) == false)
			{
				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine(
					string.Format(TheEvent.CommandText, 
					state.Map.MapName, newMapName));

				XleCore.TextArea.PrintLine();
				XleCore.Wait(500);
			}
		
			return base.OnStepOnImpl(state, ref cancel);
		}
	}
}
