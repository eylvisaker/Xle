using AgateLib.Mathematics.Geometry;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.Maps.Dungeons
{
    public class DungeonCombat
    {
        public DungeonCombat()
        {
            Monsters = new List<DungeonMonster>();
        }

        public List<DungeonMonster> Monsters { get; set; }

        public DungeonMonster MonsterAt(int dungeonLevel, Point loc)
        {
            return Monsters.FirstOrDefault(m => m.DungeonLevel == dungeonLevel && m.Location == loc);
        }
    }
}
