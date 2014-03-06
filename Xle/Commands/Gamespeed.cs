using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Commands
{
	public class Gamespeed : Command 
	{
		public override void Execute(GameState state)
		{
			MenuItemList theList = new MenuItemList("1", "2", "3", "4", "5");

			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("** Change gamespeed **", XleColor.Yellow);
			XleCore.TextArea.PrintLine("    (1 is fastest)", XleColor.Yellow);
			XleCore.TextArea.PrintLine();

			state.Player.Gamespeed = 1 + XleCore.QuickMenu(theList, 2, state.Player.Gamespeed - 1);

			XleCore.TextArea.Print("Gamespeed is: ", XleColor.Yellow);
			XleCore.TextArea.PrintLine(state.Player.Gamespeed.ToString(), XleColor.White);

			XleCore.Factory.SetGameSpeed(XleCore.GameState, state.Player.Gamespeed);

			XleCore.Wait(XleCore.GameState.GameSpeed.AfterSetGamespeedTime);

		}
	}
}
