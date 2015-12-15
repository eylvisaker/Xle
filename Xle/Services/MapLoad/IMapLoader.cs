using ERY.Xle.Maps;

namespace ERY.Xle.Services.MapLoad
{
    public interface IMapLoader : IXleService
    {
        XleMap LoadMapData(int mapId);
        IMapExtender LoadMap(int mapId);
        IMapExtender LoadMap(string filename, int mapId);
    }
}
