using AgateLib;
using System;
using Xle.Maps;
using Xle.XleEventTypes.Extenders;

namespace Xle.Services.Commands.Implementation
{
    public interface IEventInteractor
    {
        bool InteractWithFirstEvent(Func<EventExtender, bool> function);
    }

    [Singleton]
    public class EventInteractor : IEventInteractor
    {
        public GameState GameState { get; set; }

        private IMapExtender MapExtender { get { return GameState.MapExtender; } }

        public bool InteractWithFirstEvent(Func<EventExtender, bool> function)
        {
            foreach (var evt in MapExtender.EventsAt(1))
            {
                if (function(evt))
                    return true;
            }

            return false;
        }
    }
}
