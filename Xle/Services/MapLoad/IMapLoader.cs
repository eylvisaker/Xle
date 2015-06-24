using ERY.Xle.Maps;

namespace ERY.Xle.Services.MapLoad
{
    public interface IMapLoader : IXleService
    {
        XleMap LoadMapData(int mapId);
        MapExtender LoadMap(int mapId);
        MapExtender LoadMap(string filename, int mapId);
    }
}
