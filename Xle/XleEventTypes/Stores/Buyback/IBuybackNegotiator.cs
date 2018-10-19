using Xle.Services;
using System;

namespace Xle.XleEventTypes.Stores.Buyback
{
    public interface IBuybackNegotiator : IXleService
    {
        Action Redraw { get; set; }
        void NegotiatePrice(Equipment item);
    }
}