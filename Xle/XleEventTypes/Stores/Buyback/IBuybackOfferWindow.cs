using ERY.Xle.Services;

namespace ERY.Xle.XleEventTypes.Stores.Buyback
{
    public interface IBuybackOfferWindow : IXleService
    {
        TextWindow TextWindow { get; set; }

        void SetOffer(int offer, int ask);
        void RejectAskingPrice(int ask, bool wayTooHigh);
    }
}