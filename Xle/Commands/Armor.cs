﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Commands
{
	public class Armor : Command
	{
		public override void Execute(GameState state)
		{
			MenuItemList theList = new MenuItemList();
			string tempstring;
			int value = 0;
			int j = 0;

			theList.Add("Nothing");

			for (int i = 1; i <= 5; i++)
			{
				if (state.Player.ArmorType(i) > 0)
				{
					tempstring = XleCore.QualityList[state.Player.ArmorQuality(i)] + " " +
								 XleCore.ArmorList[state.Player.ArmorType(i)].Name;

					theList.Add(tempstring);
					j++;

					if (state.Player.CurrentArmor == i)
						value = j;

				}

			}

			XleCore.TextArea.PrintLine("-choose above", XleColor.Cyan);

			state.Player.CurrentArmor = XleCore.SubMenu("Pick Armor", value, theList);

		}
	}
}