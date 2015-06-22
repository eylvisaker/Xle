using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Maps;
using ERY.Xle.Maps.Extenders;

namespace ERY.Xle.Services
{
    public interface IMapLoader : IXleService
    {
        XleMap LoadMapData(int mapId);
        MapExtender LoadMap(int mapId);
        MapExtender LoadMap(string filename, int mapId);
    }
}
