using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Dungeon
{
	class MarthbaneTunnels : LobDungeonBase
	{

		protected override int MonsterGroup(int dungeonLevel)
		{
			if (dungeonLevel <= 2) return 0;
			if (dungeonLevel <= 6) return 1;

			return 2;
		}

		public override DungeonMonster GetMonsterToSpawn(GameState state)
		{
			if (state.Player.DungeonLevel == 7)
				return null;

			return base.GetMonsterToSpawn(state);
		}
	}
}
