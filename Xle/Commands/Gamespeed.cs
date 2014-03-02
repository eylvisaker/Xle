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

			g.AddBottom("** Change game speed **", XleColor.Yellow);
			g.AddBottom("     (1 is fastest)", XleColor.Yellow);
			g.AddBottom("");

			state.Player.Gamespeed = 1 + XleCore.QuickMenu(theList, 2, state.Player.Gamespeed - 1);

			ColorStringBuilder builder = new ColorStringBuilder();

			builder.AddText("Gamespeed is: ", XleColor.White);
			builder.AddText(state.Player.Gamespeed.ToString(), XleColor.Yellow);


			g.AddBottom(builder);


			XleCore.Factory.SetGameSpeed(XleCore.GameState, state.Player.Gamespeed);

			XleCore.Wait(XleCore.GameState.GameSpeed.AfterSetGamespeedTime);

		}
	}
}
