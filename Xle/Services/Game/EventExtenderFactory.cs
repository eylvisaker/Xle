using AgateLib;
using System;
using Xle.Bootstrap;
using Xle.Maps;
using Xle.XleEventTypes;
using Xle.XleEventTypes.Extenders;

namespace Xle.Services.Game
{
    public interface IEventExtenderFactory 
    {
        EventExtender Create(MapExtender map, XleEvent evt, Type defaultExtender);
    }

    [Singleton]
    public class EventExtenderFactory : IEventExtenderFactory
    {
        private readonly IAgateServiceLocator serviceLocator;

        public EventExtenderFactory(IAgateServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public EventExtender Create(MapExtender map, XleEvent evt, Type defaultExtender)
        {
            // It looks like defaultExtender is no longer required.
            // That would have some cascading refactoring, removing
            // all the overrides of ExtenderType for different event types.
            var result = serviceLocator.ResolveNamed<EventExtender>(evt.ExtenderName);

            result.TheEvent = evt;

            return result;
        }
    }
}
