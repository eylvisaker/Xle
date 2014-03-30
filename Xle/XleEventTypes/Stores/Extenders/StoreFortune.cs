﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes.Stores.Extenders
{
	public class StoreFortune : StoreExtender
	{
		protected override bool SpeakImpl(GameState state)
		{
			var player = state.Player;

			MenuItemList theList = new MenuItemList("Yes", "No");
			int choice;

			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine(TheEvent.ShopName, XleColor.Green);
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("Read your fortune for " + 
				(int)(6 * TheEvent.CostFactor) + " gold?");

			choice = XleCore.QuickMenu(theList, 3, 1);

			if (choice == 0)
			{


			}

			return true;
		}
	}
}
