﻿using AgateLib;

using Xle.Maps;

namespace Xle.Blacksilver.MapExtenders.Dungeon
{
    [Transient("TaragasMines")]
    public class TaragasMines : LobDungeon
    {
        public override int GetTreasure(int dungeonLevel, int chestID)
        {
            if (chestID == 3)
                return (int)LobItem.Lute;

            return base.GetTreasure(dungeonLevel, chestID);
        }
        protected override int MonsterGroup(int dungeonLevel)
        {
            if (dungeonLevel >= 4)
                return 1;
            else
                return 0;
        }

        public override Map3DSurfaces Surfaces()
        {
            return Lob3DSurfaces.TaragasMines;
        }
    }
}
