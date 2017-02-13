using ERY.Xle.Maps.XleMapTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics.Geometry;

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

        public bool IsWallAt(Point loc)
        {
            return Map[loc.X, loc.Y] < 0x10;
        }

        public bool RevealTrapAt(Point loc)
        {
            if (Map[loc.X, loc.Y] >= 0x21 && Map[loc.X, loc.Y] < 0x2a)
            {
                Map[loc.X, loc.Y] -= 0x10;
                return true;
            }

            return false;
        }

        public DungeonMonster MonsterAt(Point loc)
        {
            return MapExtender.MonsterAt(GameState.Player.DungeonLevel, loc);
        }

        public void OnPlayerExitDungeon()
        {
            MapExtender.OnPlayerExitDungeon();
        }

        public void OnCurrentLevelChanged()
        {
            Map.CurrentLevel = GameState.Player.DungeonLevel;
        }
    }
}
