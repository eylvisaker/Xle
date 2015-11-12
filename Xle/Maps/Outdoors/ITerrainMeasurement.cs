using ERY.Xle.Services;

namespace ERY.Xle.Maps.Outdoors
{
    public interface ITerrainMeasurement : IXleService
    {
        TerrainType TerrainAt(int x, int y);
        TerrainType TerrainAtPlayer();
    }
}