using Microsoft.Xna.Framework;
using System;
using System.Threading.Tasks;
using Xle.Data;
using Xle.Menus;

namespace Xle.XleEventTypes.Stores.Extenders
{
    public class StoreBuyback : StoreFront
    {
        public IEquipmentPicker EquipmentPicker { get; set; }
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

        protected override async Task<bool> SpeakImplAsync()
        {
            await RunStore();
            return true;
        }

        private async Task RunStore()
        {
            int i = 0;
            int choice;
            int amount;

            robbing = false;

            Screen.ClearWindows();
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

            Screen.AddWindow(wind);
            Screen.AddWindow(prompt);

            wind.SetColor(XleColor.Red);
            prompt.SetColor(XleColor.Red);

            await TextArea.PrintLine();
            await TextArea.PrintLine();
            await TextArea.PrintLine("Select (0 to cancel)");
            await TextArea.PrintLine();

            MenuItemList theList = new MenuItemList("0", "1", "2");
            choice = await QuickMenu(theList, 2, 0);

            if (choice == 0)
                return;

            Screen.RemoveWindow(prompt);
            wind.Visible = false;

            ColorScheme.FrameColor = XleColor.Gray;
            ColorScheme.HorizontalLinePosition = 11;
            Title = "";
            ShowGoldText = false;

            Equipment item = null;
            TextArea.Clear();

            TextWindow questionWindow = new TextWindow { Location = new Point(5, 16) };

            Screen.AddWindow(questionWindow);

            switch (choice)
            {
                case 1:
                    questionWindow.WriteLine("What weapon will you sell me?");
                    item = await EquipmentPicker.PickWeapon(GameState, null, ColorScheme.BackColor);
                    break;

                case 2:
                    questionWindow.WriteLine("What armor will you sell me?");
                    item = await EquipmentPicker.PickArmor(GameState, null, ColorScheme.BackColor);
                    break;
            }

            if (item == null)
                return;

            Screen.RemoveWindow(questionWindow);

            ColorScheme.HorizontalLinePosition = 14;
            ColorScheme.TextAreaBackColor = XleColor.Black;

            Title = "Buy-back shop";
            wind.Visible = true;
            wind.SetColor(XleColor.White);
            wind.Location = new Point(9, 8);

            var ta = TextArea;

            TextWindow offerText = new TextWindow();
            offerText.Location = new Point(2, 16);

            int charm = Player.Attribute[Attributes.charm];
            charm = Math.Min(charm, 80);

            int maxAccept = (int)(item.Price(Data) * Math.Pow(charm, .7) / 11);
            int offer = (int)((6 + Random.NextDouble()) * maxAccept / 14.0);

            choice = await MakeOffer(item, offer, false);

            if (choice == 0)
            {
                await CompleteSale(offer, item);
                return;
            }
            int ask = 0;

            Screen.AddWindow(offerText);

            SetOfferText(offerText, offer, ask);

            ask = await GetAskingPrice();

            if (ask == 0)
            {
                await ta.PrintLine("\n\n\n\nSee you later.\n");
                return;
            }
            if (ask < 1.5 * offer)
            {
                await CompleteSale(ask, item);
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

                SetAskRejectPrice(offerText, ask, WayTooHigh(ask, offer, maxAccept));
                choice = await MakeOffer(item, offer, finalOffer);

                if (choice == 0)
                {
                    await CompleteSale(offer, item);
                    return;
                }
                else if (finalOffer)
                {
                    await MaybeDealLater();
                    return;
                }

                SetOfferText(offerText, offer, lastAsk);
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
                    await CompleteSale(ask, item);
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

        private async Task<int> GetAskingPrice()
        {
            TextArea.Clear();

            return await NumberPicker.ChooseNumber(32767);
        }

        private async Task<int> MakeOffer(Equipment item, int offer, bool finalOffer)
        {
            var ta = TextArea;

            ta.Clear();
            await ta.PrintLine("I'll give " + offer + " gold for your");
            await ta.Print(item.NameWithQuality(Data));

            if (finalOffer)
            {
                await ta.PrintLine(" -final offer!!!", XleColor.Yellow);
            }
            else
                await ta.PrintLine(".");

            return await QuickMenuService.QuickMenuYesNo(true);
        }

        private bool WayTooHigh(double ask, int offer, int maxAccept)
        {
            return (ask / offer) > 1.4 && (ask / maxAccept > 1.3);
        }

        private void SetAskRejectPrice(TextWindow offerWind, int ask, bool wayTooHigh)
        {
            var clr = wayTooHigh ? XleColor.Yellow : XleColor.Cyan;

            offerWind.Clear();
            offerWind.WriteLine(" " + ask + " is " +
                (wayTooHigh ? "way " : "") + "too high!", clr);
        }

        private void SetOfferText(TextWindow offerWind, int offer, int ask)
        {
            offerWind.Clear();
            offerWind.Write("My latest offer: ", XleColor.White);
            offerWind.WriteLine(offer.ToString(), XleColor.Cyan);

            if (ask > 0)
            {
                offerWind.Write("You asked for: ");
                offerWind.WriteLine(ask.ToString(), XleColor.Cyan);
            }
            else
                offerWind.WriteLine();

            offerWind.Write("What will you sell for? ");
            offerWind.Write("(0 to quit)", XleColor.Purple);
        }

        private async Task ComeBackWhenSerious()
        {
            TextArea.Clear();
await             TextArea.PrintLine("Come back when you're serious.");

            await Wait(1500);
        }
        private async Task MaybeDealLater()
        {
            TextArea.Clear();
            await TextArea.PrintLine("Maybe we can deal later.");

            await Wait(1500);
        }

        private async Task CompleteSale(int offer, Equipment item)
        {
            TextArea.Clear();
            await TextArea.PrintLine("It's a deal!");
            await TextArea.PrintLine(item.BaseName(Data) + " sold for " + offer + " gold.");

            Player.Gold += offer;
            Player.RemoveEquipment(item);

            await StoreSound(LotaSound.Sale);
        }
    }
}
