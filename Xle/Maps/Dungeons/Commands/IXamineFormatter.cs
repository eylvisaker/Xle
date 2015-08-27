using ERY.Xle.Services;

namespace ERY.Xle.Maps.Dungeons.Commands
{
    public interface IXamineFormatter : IXleService
    {
        void PrintNothingUnusualInSight();
        void PrintHiddenObjectsDetected();
        void DescribeTile(DungeonTile tripWire, int distance);
        void DescribeMonster(DungeonMonster monster);
    }
}