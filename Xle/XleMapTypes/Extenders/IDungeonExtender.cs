using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.Extenders
{
	public interface IDungeonExtender
	{
		void OnPlayerExitDungeon(Player player);

		void OnBeforeGiveItem(Player player, ref int treasure, ref bool handled);

		void OnBeforeOpenBox(Player player, ref bool handled);
	}
}
