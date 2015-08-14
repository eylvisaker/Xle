using ERY.Xle.Maps.XleMapTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.Maps.Dungeons
{
    public class DungeonAdapter : IDungeonAdapter
    {
        public GameState GameState { get; set; }

        Dungeon Map { get { return (Dungeon)GameState.Map; } }

        DungeonExtender MapExtender {  get { return (DungeonExtender)GameState.MapExtender; } }

        public int ChestValueAt(int x, int y, int level = -1)
        {
            level = NormalizeLevel(level);
            var tile = Map[x, y, level];

            if (tile >= 0x30 && tile <= 0x3f)
                return tile - 0x30;

            else
                throw new InvalidOperationException();
        }

        public DungeonTile TileAt(int x, int y, int level = -1)
        {
            level = NormalizeLevel(level);

            var result = Map[x, y, level];

            if (result >= 0x30 && result <= 0x3f)
                return DungeonTile.Chest;

            return (DungeonTile)result;
        }

        private int NormalizeLevel(int level)
        {
            if (level == -1)
                level = GameState.Player.DungeonLevel;
            return level;
        }

        public void ClearSpace(int x, int y, int level = -1)
        {
            level = NormalizeLevel(level);

            Map[x, y, level] = 0x10;
        }

        public int GetTreasure(int chestValue, int level = -1)
        {
            level = NormalizeLevel(level);

            return MapExtender.GetTreasure(level + 1, chestValue);
        }
    }
}
