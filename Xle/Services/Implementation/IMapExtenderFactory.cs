using ERY.Xle.Maps;
using ERY.Xle.Maps.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Services.Implementation
{
    public interface IMapExtenderFactory : IXleFactory
    {
        MapExtender CreateMapExtender(XleMap map);
    }
}
