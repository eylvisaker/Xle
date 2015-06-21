﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Dungeon
{
    public class TaragasMines : LobDungeon
    {
        public override int GetTreasure(GameState state, int dungeonLevel, int chestID)
        {
            if (chestID == 3)
                return (int)LobItem.Lute;

            return base.GetTreasure(state, dungeonLevel, chestID);
        }
        protected override int MonsterGroup(int dungeonLevel)
        {
            if (dungeonLevel >= 4)
                return 1;
            else
                return 0;
        }

        public override Maps.Map3DSurfaces Surfaces(GameState state)
        {
            return Lob3DSurfaces.TaragasMines;
        }
    }
}
