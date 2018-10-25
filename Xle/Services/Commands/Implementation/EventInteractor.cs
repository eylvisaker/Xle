using AgateLib;
using System;
using System.Threading.Tasks;
using Xle.Maps;
using Xle.XleEventTypes.Extenders;

namespace Xle.Services.Commands.Implementation
{
    public interface IEventInteractor
    {
        Task<bool> InteractWithFirstEvent(Func<EventExtender, Task<bool>> function);
    }

    [Singleton]
    public class EventInteractor : IEventInteractor
    {
        public GameState GameState { get; set; }

        private IMapExtender MapExtender { get { return GameState.MapExtender; } }

        public async Task<bool> InteractWithFirstEvent(Func<EventExtender, Task<bool>> function)
        {
            foreach (var evt in MapExtender.EventsAt(1))
            {
                if (await function(evt))
                    return true;
            }

            return false;
        }
    }
}
