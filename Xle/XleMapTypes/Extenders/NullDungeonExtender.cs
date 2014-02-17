﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.Extenders
{
	public class NullDungeonExtender : IDungeonExtender
	{
		public void OnPlayerExitDungeon(Player player)
		{
		}

		public void OnBeforeGiveItem(Player player, ref int treasure, ref bool handled)
		{
		}

		public void OnBeforeOpenBox(Player player, ref bool handled)
		{
		}

		public void OnLoad(Player player)
		{
		}



		public int GetOutsideTile(AgateLib.Geometry.Point playerPoint, int x, int y)
		{
			return 0;
		}


		public void GetBoxColors(out AgateLib.Geometry.Color boxColor, out AgateLib.Geometry.Color innerColor, out AgateLib.Geometry.Color fontColor, out int vertLine)
		{
			boxColor = XleColor.Gray;
			innerColor = XleColor.LightGreen;
			fontColor = XleColor.Cyan;
			vertLine = 15 * 16;
		}
	}
}