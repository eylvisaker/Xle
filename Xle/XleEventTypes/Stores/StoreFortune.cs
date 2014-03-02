using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes
{

	public class StoreFortune : Store
	{
		public override bool Speak(GameState state)
		{
			var player = state.Player;

			if (CheckLoan(player, true))
				return true;

			MenuItemList theList = new MenuItemList("Yes", "No");
			int choice;

			g.AddBottom(this.ShopName, XleColor.Green);
			g.AddBottom("");
			g.AddBottom("Read your fortune for " + (int)(6 * CostFactor) + " gold?");

			choice = XleCore.QuickMenu(theList, 3, 1);

			if (choice == 0)
			{


			}

			return true;
		}
	}
}
