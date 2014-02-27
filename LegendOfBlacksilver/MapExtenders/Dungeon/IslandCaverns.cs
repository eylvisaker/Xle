using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Dungeon
{
	class IslandCaverns : LobDungeonBase
	{
		public override int GetTreasure(GameState state, int dungeonLevel, int chestID)
		{
			if (dungeonLevel == 3 && chestID == 3)
				return (int)LobItem.CrystalRing;

			return 0;
		}
	}
}
