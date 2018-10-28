using AgateLib;
using System;
using System.Threading.Tasks;
using Xle.Data;
using Xle.Services.Game;
using Xle.Services.Menus;
using Xle.Services.XleSystem;

namespace Xle.XleEventTypes.Stores.Buyback
{
    public interface IBuybackNegotiator
    {
        Task NegotiatePrice(Equipment item);
    }

    [Transient, InjectProperties]
    public class BuybackNegotiator : IBuybackNegotiator
    {
        public BuybackNegotiator()
        {
            Redraw = () => { };
        }

        public IBuybackFormatter BuybackFormatter { get; set; }
        public IBuybackOfferWindow OfferWindow { get; set; }
        public INumberPicker NumberPicker { get; set; }
        public IQuickMenu QuickMenu { get; set; }
        public IXleGameControl GameControl { get; set; }
        public ISoundMan SoundMan { get; set; }
        public GameState GameState { get; set; }
        public XleData Data { get; set; }
        public Random Random { get; set; }

        private Player Player { get { return GameState.Player; } }

        public Action Redraw { get; set; }

        private Task Wait(int howlong) => GameControl.WaitAsync(howlong);

        public async Task NegotiatePrice(Equipment item)
        {
            int charm = Player.Attribute[Attributes.charm];
            charm = Math.Min(charm, 80);

            int maxAccept = (int)(item.Price(Data) * Math.Pow(charm, .7) / 11);
            int offer = (int)((6 + Random.NextDouble()) * maxAccept / 14.0);

            int choice = await MakeOffer(item, offer, false);

            if (choice == 0)
            {
                await CompleteSale(item, offer);
                return;
            }
            int ask = 0;

            OfferWindow.SetOffer(offer, ask);

            ask = await GetAskingPrice();

            if (ask == 0)
            {
                await BuybackFormatter.SeeYouLater();
                return;
            }
            if (ask < 1.5 * offer)
            {
                await CompleteSale(item, ask);
                return;
            }

            int spread = maxAccept - offer;

            if (ask > spread + maxAccept)
            {
                await ComeBackWhenSerious();
                return;
            }

            spread = ask - offer;
            double scale = maxAccept / (double)spread;
            offer = (int)(offer + (1 + Random.NextDouble() * 5) * scale);
            maxAccept = spread;

            if (offer >= ask)
                offer = ask - 1;

            int lastAsk = ask;

            do
            {
                bool finalOffer = false;

                OfferWindow.RejectAskingPrice(ask, IsAskWayTooHigh(ask, offer, maxAccept));
                choice = await MakeOffer(item, offer, finalOffer);

                if (choice == 0)
                {
                    await CompleteSale(item, offer);
                    return;
                }
                else if (finalOffer)
                {
                    await MaybeDealLater();
                    return;
                }

                OfferWindow.SetOffer(offer, lastAsk);
                ask = await GetAskingPrice();

                if (ask == 0)
                {
                    await MaybeDealLater();
                    return;
                }

                if (ask == lastAsk ||
                    (ask > lastAsk && Random.NextDouble() < 0.5))
                {
                    await ComeBackWhenSerious();
                    return;
                }

                double diff = lastAsk - ask;
                if (diff == 0) diff = Random.NextDouble() * 3;

                if (diff / maxAccept < 0.03)
                    diff /= 1.3;

                lastAsk = ask;
                spread = (int)(offer + diff / 1.2 + Random.NextDouble() * diff / 1.6);

                if (spread > ask - 2 && Random.NextDouble() < .5)
                {
                    await CompleteSale(item, ask);
                    return;
                }
                if (spread >= ask)
                {
                    finalOffer = true;
                }
                else
                {
                    offer = spread;

                    if (ask - offer < 3)
                        finalOffer = true;

                    if (offer <= 0)
                    {
                        await ComeBackWhenSerious();
                        return;
                    }
                }

            } while (true);
        }


        private Task<int> GetAskingPrice()
        {
            BuybackFormatter.ClearTextArea();

            return NumberPicker.ChooseNumber(32767);
        }

        private Task<int> MakeOffer(Equipment item, int offer, bool finalOffer)
        {
            BuybackFormatter.Offer(item, offer, finalOffer);

            return QuickMenu.QuickMenuYesNo(true);
        }

        private bool IsAskWayTooHigh(double ask, int offer, int maxAccept)
        {
            return (ask / offer) > 1.4 && (ask / maxAccept > 1.3);
        }

        private void SetAskRejectPrice(TextWindow offerWind, int ask, bool wayTooHigh)
        {
            OfferWindow.RejectAskingPrice(ask, wayTooHigh);
        }

        private void SetOfferText(TextWindow offerWind, int offer, int ask)
        {
            OfferWindow.SetOffer(offer, ask);
        }

        private async Task ComeBackWhenSerious()
        {
            await BuybackFormatter.ComeBackWhenSerious();
            await Wait(1500);
        }

        private async Task MaybeDealLater()
        {
            await BuybackFormatter.MaybeDealLater();
            await Wait(1500);
        }

        private async Task CompleteSale(Equipment item, int offer)
        {
            await BuybackFormatter.CompleteSale(item, offer);

            Player.Gold += offer;
            Player.RemoveEquipment(item);

            await GameControl.PlaySoundWait(LotaSound.Sale);
        }

    }
}
