namespace ERY.Xle.Services.Menus
{
    public interface IMuseumCoinSale : IXleService
    {
        void OfferMuseumCoin();

        bool RollToOfferCoin();
    }
}
