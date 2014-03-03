﻿using System;
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

			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("Would you like to save");
			XleCore.TextArea.PrintLine("the game in progress?");
			XleCore.TextArea.PrintLine();

			choice = XleCore.QuickMenu(menu, 2);

			if (choice == 0)
			{
				player.SavePlayer();

				saved = true;

				XleCore.TextArea.PrintLine("Game Saved.");
				XleCore.TextArea.PrintLine();
			}
			else
			{
				ColorStringBuilder builder = new ColorStringBuilder();

				builder.AddText("Game ", XleColor.White);
				builder.AddText("not", XleColor.Yellow);
				builder.AddText(" saved.", XleColor.White);

				g.AddBottom(builder);
				XleCore.TextArea.PrintLine();
			}

			XleCore.Wait(1500);

			XleCore.TextArea.PrintLine("Quit and return to title screen?");

			if (saved == false)
				XleCore.TextArea.PrintLine("Unsaved progress will be lost.", XleColor.Yellow);
			else
				XleCore.TextArea.PrintLine();

			XleCore.TextArea.PrintLine();

			choice = XleCore.QuickMenu(menu, 2, 1);

			if (choice == 0)
			{
				XleCore.ReturnToTitle = true;
			}
		}
	}
}
