
using ERY.Xle.Data;
using Microsoft.Xna.Framework;

namespace ERY.Xle.Maps
{
    public class DungeonMonster
    {
        public DungeonMonster(DungeonMonsterData data)
        {
            Data = data;
        }

        public int MonsterID { get { return Data.ID; } }
        public DungeonMonsterData Data { get; private set; }
        public int DungeonLevel { get; set; }
        public Point Location { get; set; }

        public int HP { get; set; }

        public string Name { get { return Data.Name; } }

        public bool KillFlashImmune { get; set; }
    }
}
