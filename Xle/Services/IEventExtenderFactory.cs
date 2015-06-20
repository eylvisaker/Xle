using ERY.Xle.Maps.Extenders;
using ERY.Xle.XleEventTypes;
using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Services
{
    public interface IEventExtenderFactory : IXleFactory
    {
        EventExtender Create(MapExtender map, XleEvent evt, Type defaultExtender);
    }
}
