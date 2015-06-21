using ERY.Xle.Maps.Extenders;
using ERY.Xle.Maps.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Services.Implementation
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
