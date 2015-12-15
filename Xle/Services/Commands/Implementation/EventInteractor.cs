using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERY.Xle.Maps;
using ERY.Xle.XleEventTypes.Extenders;

namespace ERY.Xle.Services.Commands.Implementation
{
    public class EventInteractor : IEventInteractor
    {
        public GameState GameState { get; set; }

        IMapExtender MapExtender { get { return GameState.MapExtender; } }

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
