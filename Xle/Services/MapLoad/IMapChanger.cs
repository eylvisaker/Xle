using ERY.Xle.Maps.Extenders;

namespace ERY.Xle.Services.MapLoad
{
    public interface IMapChanger : IXleService
    {
        void SetMap(MapExtender map);

        void ChangeMap(Player player, int mapId, int entryPoint);

        void ChangeMap(Player player, int mapId, AgateLib.Geometry.Point targetPoint);

        void ReturnToPreviousMap();
    }
}
