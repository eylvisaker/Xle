using System;

using ERY.Xle.Maps.Extenders;
using ERY.Xle.XleEventTypes;
using ERY.Xle.XleEventTypes.Extenders;

namespace ERY.Xle.Services.Game
{
    public interface IEventExtenderFactory : IXleFactory
    {
        EventExtender Create(MapExtender map, XleEvent evt, Type defaultExtender);
    }
}
