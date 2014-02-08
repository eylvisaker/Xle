using System;
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
	}
}
