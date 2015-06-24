using ERY.Xle.Maps;
using ERY.Xle.Services.Rendering.Maps;

namespace ERY.Xle.Services.Rendering
{
    public interface IMapRendererFactory : IXleFactory
    {
        DungeonRenderer DungeonRenderer(MapExtender map, string name = "");
        OutsideRenderer OutsideRenderer(MapExtender map, string name = "");
        MuseumRenderer MuseumRenderer(MapExtender map, string name = "");
        TempleRenderer TempleRenderer(MapExtender map, string name = "");
        TownRenderer TownRenderer(MapExtender map, string name = "");
        CastleRenderer CastleRenderer(MapExtender map, string name = "");
    }
}
