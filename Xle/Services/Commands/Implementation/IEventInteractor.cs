using Xle.XleEventTypes;
using Xle.XleEventTypes.Extenders;
using System;

namespace Xle.Services.Commands.Implementation
{
    public interface IEventInteractor : IXleService
    {
        bool InteractWithFirstEvent(Func<EventExtender, bool> function);
    }
}