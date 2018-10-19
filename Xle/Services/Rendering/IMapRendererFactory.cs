using Xle.Maps;
using Xle.Services.Rendering.Maps;

namespace Xle.Services.Rendering
{
    public interface IMapRendererFactory : IXleFactory
    {
        DungeonRenderer DungeonRenderer(MapExtender map, string name = null);
        OutsideRenderer OutsideRenderer(MapExtender map, string name = null);
        MuseumRenderer MuseumRenderer(MapExtender map, string name = null);
        TempleRenderer TempleRenderer(MapExtender map, string name = null);
        TownRenderer TownRenderer(MapExtender map, string name = null);
        CastleRenderer CastleRenderer(MapExtender map, string name = null);
    }
}
