using Xle.Services;

namespace Xle.Maps.Dungeons.Commands
{
    public interface IXamineFormatter : IXleService
    {
        void PrintNothingUnusualInSight();
        void PrintHiddenObjectsDetected();
        void DescribeTile(DungeonTile tripWire, int distance);
        void DescribeMonster(DungeonMonster monster);
    }
}