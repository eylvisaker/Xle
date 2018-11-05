using AgateLib;
using System;
using System.Threading.Tasks;
using Xle.Maps;
using Xle.XleEventTypes.Extenders;

namespace Xle.Commands.Implementation
{
    public interface IEventInteractor
    {
        Task<bool> InteractWithFirstEvent(Func<IEventExtender, Task<bool>> function);
    }

    [Singleton]
    public class EventInteractor : IEventInteractor
    {
        private readonly GameState gameState;

        public EventInteractor(GameState gameState)
        {
            this.gameState = gameState;
        }

        private IMapExtender MapExtender => gameState.MapExtender;

        public async Task<bool> InteractWithFirstEvent(Func<IEventExtender, Task<bool>> function)
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
