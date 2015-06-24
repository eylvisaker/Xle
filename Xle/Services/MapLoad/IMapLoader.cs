using ERY.Xle.Maps;
using ERY.Xle.Maps.Extenders;

namespace ERY.Xle.Services.MapLoad
{
    public interface IMapLoader : IXleService
    {
        XleMap LoadMapData(int mapId);
        MapExtender LoadMap(int mapId);
        MapExtender LoadMap(string filename, int mapId);
    }
}
