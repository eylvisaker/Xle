using Xle.Services;

namespace Xle.XleEventTypes.Stores.Buyback
{
    public interface IBuybackOfferWindow : IXleService
    {
        TextWindow TextWindow { get; set; }

        void SetOffer(int offer, int ask);
        void RejectAskingPrice(int ask, bool wayTooHigh);
    }
}