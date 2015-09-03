using ERY.Xle.XleEventTypes;
using ERY.Xle.XleEventTypes.Extenders;
using System;

namespace ERY.Xle.Services.Commands.Implementation
{
    public interface IEventInteractor : IXleService
    {
        bool InteractWithFirstEvent(Func<EventExtender, bool> function);
    }
}