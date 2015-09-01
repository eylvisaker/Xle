using AgateLib.Geometry;
using ERY.Xle.Services;

namespace ERY.Xle.Maps.Dungeons
{
    public interface IDungeonAdapter : IXleService
    {
        DungeonTile TileAt(int x, int y, int level = -1);
        int ChestValueAt(int x, int y, int level = -1);
        void ClearSpace(int x, int y, int level = -1);
        int GetTreasure(int chestValue, int level = -1);
        bool IsWallAt(Point loc);
        bool RevealTrapAt(Point loc);
        DungeonMonster MonsterAt(Point loc);
        void OnPlayerExitDungeon();
        void OnCurrentLevelChanged();
    }
}