using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;

using ERY.Xle.Data;
using ERY.Xle.Services;
using ERY.Xle.Services.Menus;
using ERY.Xle.XleEventTypes.Stores.Extenders.BuybackImplementation;

namespace ERY.Xle.XleEventTypes.Stores.Extenders
{
    public class StoreBuyback : StoreFront
    {
        public IBuybackFormatter BuybackFormatter { get; set; }
        public IEquipmentPicker EquipmentPicker { get; set; }
        public IBuybackOfferWindow OfferWindow { get; set; }
        public XleData Data { get; set; }

        public override bool AllowInteractionWhenLoanOverdue { get { return true; } }

        protected override void InitializeColorScheme(ColorScheme cs)
        {
            cs.BackColor = XleColor.Pink;
            cs.FrameColor = XleColor.Yellow;
            cs.FrameHighlightColor = XleColor.Yellow;
            cs.TextAreaBackColor = XleColor.Brown;
            cs.BorderColor = XleColor.Red;
        }

        protected override bool SpeakImpl()
        {
            RunStore();
            return true;
        }

        void RunStore()
        {
            robbing = false;

            ClearWindow();
            Title = TheEvent.ShopName;

            var wind = new TextWindow();
            wind.Location = new Point(9, 4);

            wind.WriteLine("I will happily purchase");
            wind.WriteLine("your used arms and armor");

            var prompt = new TextWindow();

            prompt.Location = new Point(9, 9);
            prompt.WriteLine("Choose items to sell:");
            prompt.WriteLine();
            prompt.WriteLine(" 1.  Weapons");
            prompt.WriteLine(" 2.  Armor");

            Windows.Add(wind);
            Windows.Add(prompt);

            wind.SetColor(XleColor.Red);
            prompt.SetColor(XleColor.Red);

            BuybackFormatter.InitialMenuPrompt();

            MenuItemList theList = new MenuItemList("0", "1", "2");
            int choice = QuickMenu(theList, 2, 0);

            if (choice == 0)
                return;

            Windows.Remove(prompt);
            wind.Visible = false;

            ColorScheme.FrameColor = XleColor.Gray;
            ColorScheme.HorizontalLinePosition = 11;
            Title = "";
            ShowGoldText = false;

            Equipment item = null;
            TextArea.Clear();

            TextWindow questionWindow = new TextWindow { Location = new Point(5, 16) };

            Windows.Add(questionWindow);

            switch (choice)
            {
                case 1:
                    questionWindow.WriteLine("What weapon will you sell me?");
                    item = EquipmentPicker.PickWeapon(GameState, null, ColorScheme.BackColor);
                    break;

                case 2:
                    questionWindow.WriteLine("What armor will you sell me?");
                    item = EquipmentPicker.PickArmor(GameState, null, ColorScheme.BackColor);
                    break;
            }

            if (item == null)
                return;

            Windows.Remove(questionWindow);

            wind.Visible = true;
            wind.SetColor(XleColor.White);
            wind.Location = new Point(9, 8);

            ColorScheme.HorizontalLinePosition = 14;
            ColorScheme.TextAreaBackColor = XleColor.Black;

            Title = "Buy-back shop";

            NegotiatePrice(item);
        }

        private void NegotiatePrice(Equipment item)
        {
            int charm = Player.Attribute[Attributes.charm];
            charm = Math.Min(charm, 80);

            int maxAccept = (int)(item.Price(Data) * Math.Pow(charm, .7) / 11);
            int offer = (int)((6 + Random.NextDouble()) * maxAccept / 14.0);

            int choice = MakeOffer(item, offer, false);

            if (choice == 0)
            {
                CompleteSale(item, offer);
                return;
            }
            int ask = 0;
            InitializeOfferWindow();

            OfferWindow.SetOffer(offer, ask);

            ask = GetAskingPrice();

            if (ask == 0)
            {
                BuybackFormatter.SeeYouLater();
                return;
            }
            if (ask < 1.5 * offer)
            {
                CompleteSale(item, ask);
                return;
            }

            int spread = maxAccept - offer;

            if (ask > spread + maxAccept)
            {
                ComeBackWhenSerious();
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
                choice = MakeOffer(item, offer, finalOffer);

                if (choice == 0)
                {
                    CompleteSale(item, offer);
                    return;
                }
                else if (finalOffer)
                {
                    MaybeDealLater();
                    return;
                }

                OfferWindow.SetOffer(offer, lastAsk);
                ask = GetAskingPrice();

                if (ask == 0)
                {
                    MaybeDealLater();
                    return;
                }

                if (ask == lastAsk ||
                    (ask > lastAsk && Random.NextDouble() < 0.5))
                {
                    ComeBackWhenSerious();
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
                    CompleteSale(item, ask);
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
                        ComeBackWhenSerious();
                        return;
                    }
                }

            } while (true);
        }

        private void InitializeOfferWindow()
        {
            TextWindow offerText = new TextWindow();
            offerText.Location = new Point(2, 16);

            Windows.Add(offerText);

            OfferWindow.TextWindow = offerText;
        }

        private int GetAskingPrice()
        {
            TextArea.Clear();

            return NumberPicker.ChooseNumber(32767);
        }

        private int MakeOffer(Equipment item, int offer, bool finalOffer)
        {
            BuybackFormatter.Offer(item, offer, finalOffer);
            
            return QuickMenuService.QuickMenuYesNo(true);
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

        private void ComeBackWhenSerious()
        {
            BuybackFormatter.ComeBackWhenSerious();
          
            Wait(1500);
        }
        private void MaybeDealLater()
        {
            BuybackFormatter.MaybeDealLater();
            Wait(1500);
        }

        private void CompleteSale(Equipment item, int offer)
        {
            BuybackFormatter.CompleteSale(item, offer);

            Player.Gold += offer;
            Player.RemoveEquipment(item);

            StoreSound(LotaSound.Sale);
        }

    }
}
