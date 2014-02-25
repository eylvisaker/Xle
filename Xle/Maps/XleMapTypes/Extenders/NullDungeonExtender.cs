﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.Extenders
{
	public class NullDungeonExtender : NullMapExtender, IDungeonExtender
	{
		public virtual void OnPlayerExitDungeon(Player player)
		{
		}

		public virtual void OnBeforeGiveItem(Player player, ref int treasure, ref bool handled)
		{
		}

		public virtual void OnBeforeOpenBox(Player player, ref bool handled)
		{
		}

		public virtual void OnLoad(Player player)
		{
		}

		public virtual string TrapName(int val)
		{
			switch (val)
			{
				case 0x11: return "ceiling hole";
				case 0x12: return "floor hole";
				case 0x13: return "poison gas vent";
				case 0x14: return "slime splotch";
				case 0x15: return "trip wire";
				default: throw new ArgumentException();
			}
		}


		public override void GetBoxColors(out AgateLib.Geometry.Color boxColor, out AgateLib.Geometry.Color innerColor, out AgateLib.Geometry.Color fontColor, out int vertLine)
		{
			boxColor = XleColor.Gray;
			innerColor = XleColor.LightGreen;
			fontColor = XleColor.Cyan;
			vertLine = 15 * 16;
		}
	}
}
