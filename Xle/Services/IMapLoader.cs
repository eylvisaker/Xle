using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Maps;

namespace ERY.Xle.Services
{
    public interface IMapLoader : IXleService
    {
        XleMap LoadMap(int mapId);
    }
}
