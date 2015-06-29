using AgateLib.Geometry;

using ERY.Xle.Maps;

namespace ERY.Xle.Services.MapLoad
{
    public interface IMapChanger : IXleService
    {
        void SetMap(MapExtender map);

        void ChangeMap(int mapId, int entryPoint);
        void ChangeMap(int mapId, Point targetPoint);

        void ReturnToPreviousMap();
    }
}
