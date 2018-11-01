using AgateLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xle.Maps;

namespace Xle.Blacksilver.MapExtenders.Dungeon
{
    [Transient("DeathspireChasm")]
	public class DeathspireChasm : LobDungeon
	{
		protected override int MonsterGroup(int dungeonLevel)
		{
			if (dungeonLevel <= 3) return 0;
			if (dungeonLevel <= 7) return 1;

			return 2;
		}

		public override Map3DSurfaces Surfaces()
		{
			return Lob3DSurfaces.DeathspireChasm;
		}
	}
}
