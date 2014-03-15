using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Commands
{
	public class Weapon : Command
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
				if (state.Player.WeaponType(i) > 0)
				{
					tempstring = XleCore.Data.QualityList[state.Player.WeaponQuality(i)] + " " +
								 XleCore.Data.WeaponList[state.Player.WeaponType(i)].Name;

					theList.Add(tempstring);
					j++;

					if (state.Player.CurrentWeaponIndex == i)
					{
						value = j;
					}
				}

			}

			XleCore.TextArea.PrintLine("-choose above", XleColor.Cyan);

			state.Player.CurrentWeaponIndex = XleCore.SubMenu("Pick Weapon", value, theList);
		}
	}
}
