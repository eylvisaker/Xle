using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Dungeon
{
	class TaragasMines : LobDungeonBase
	{
		public override int GetTreasure(GameState state, int dungeonLevel, int chestID)
		{
			if (chestID == 3)
				return (int)LobItem.Lute;

			return base.GetTreasure(state, dungeonLevel, chestID);
		}
	}
}
