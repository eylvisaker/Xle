using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Dungeon
{
	class DeathspireChasm : LobDungeonBase
	{
		protected override int MonsterGroup(int dungeonLevel)
		{
			if (dungeonLevel <= 3) return 0;
			if (dungeonLevel <= 7) return 1;

			return 2;
		}
	}
}
