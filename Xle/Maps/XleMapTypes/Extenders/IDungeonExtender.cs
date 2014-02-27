using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.Extenders
{
	public interface IDungeonExtender : IMapExtender
	{
		void OnPlayerExitDungeon(Player player);

		void OnBeforeGiveItem(Player player, ref int treasure, ref bool handled);

		void OnBeforeOpenBox(Player player, ref bool handled);

		void OnLoad(Player player);

		string TrapName(int val);

		int GetTreasure(GameState gameState, int CurrentLevel, int val);
	}
}
