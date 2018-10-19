using System;

using Xle.Maps;
using Xle.XleEventTypes;
using Xle.XleEventTypes.Extenders;

namespace Xle.Services.Game
{
    public interface IEventExtenderFactory : IXleFactory
    {
        EventExtender Create(MapExtender map, XleEvent evt, Type defaultExtender);
    }
}
