using ERY.Xle.Services;

namespace ERY.Xle.XleEventTypes.Stores.Extenders
{
    public interface IBuybackFormatter : IXleService
    {
        void Offer(Equipment item, int offer, bool finalOffer);
        void ComeBackWhenSerious();
        void MaybeDealLater();
        void CompleteSale(Equipment item, int amount);
        void SeeYouLater();
        void InitialMenuPrompt();
    }
}