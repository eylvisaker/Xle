using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Commands
{
	public class End : Command
	{
		public override void Execute(GameState state)
		{
			var player = state.Player;

			MenuItemList menu = new MenuItemList("Yes", "No");
			int choice;
			bool saved = false;

			g.AddBottom("");
			g.AddBottom("Would you like to save");
			g.AddBottom("the game in progress?");
			g.AddBottom("");

			choice = XleCore.QuickMenu(menu, 2);

			if (choice == 0)
			{
				player.SavePlayer();

				saved = true;

				g.AddBottom("Game Saved.");
				g.AddBottom("");
			}
			else
			{
				ColorStringBuilder builder = new ColorStringBuilder();

				builder.AddText("Game ", XleColor.White);
				builder.AddText("not", XleColor.Yellow);
				builder.AddText(" saved.", XleColor.White);

				g.AddBottom(builder);
				g.AddBottom("");
			}

			XleCore.Wait(1500);

			g.AddBottom("Quit and return to title screen?");

			if (saved == false)
				g.AddBottom("Unsaved progress will be lost.", XleColor.Yellow);
			else
				g.AddBottom("");

			g.AddBottom("");

			choice = XleCore.QuickMenu(menu, 2, 1);

			if (choice == 0)
			{
				XleCore.ReturnToTitle = true;
			}
		}
	}
}
