using Xle.Data;
using Xle.Services.Game;
using Xle.Services.ScreenModel;
using Xle.Services.XleSystem;
using System;

namespace Xle.Services.Menus.Implementation
{
    public abstract class MuseumCoinSale : IMuseumCoinSale
    {
        public Random Random { get; set; }
        public ITextArea TextArea { get; set; }
        public IXleGameControl GameControl { get; set; }
        public GameState GameState { get; set; }
        public ISoundMan SoundMan { get; set; }
        public IQuickMenu QuickMenu { get; set; }
        public XleData Data { get; set; }

        public Player Player { get { return GameState.Player; } }

        public bool RollToOfferCoin()
        {
            return Random.Next(1000) < 1000 * ChanceToOfferCoin;
        }

        public void OfferMuseumCoin()
        {
            int coin = -1;
            MenuItemList menu = new MenuItemList("Yes", "No");

            coin = NextMuseumCoinOffer();

            if (coin == -1)
                return;

            int amount = 50 + (int)(Random.NextDouble() * 20 * Player.Level);

            if (amount > Player.Gold)
                amount /= 2;

            SoundMan.PlaySound(LotaSound.Question);

            TextArea.PrintLine("Would you like to buy a ");
            GameControl.Wait(1);

            TextArea.PrintLine("museum coin for " + amount.ToString() + " gold?");
            GameControl.Wait(1);

            TextArea.PrintLine();
            GameControl.Wait(1);

            int choice = QuickMenu.QuickMenu(menu, 3, 0);

            if (choice == 0)
            {
                if (Player.Spend(amount))
                {
                    string coinName = Data.ItemList[coin].Name;

                    TextArea.PrintLine("Use this " + coinName + " well!");

                    Player.Items[coin] += 1;

                    SoundMan.PlaySound(LotaSound.Sale);
                }
                else
                {
                    TextArea.PrintLine("Not enough gold.");
                    SoundMan.PlaySound(LotaSound.Medium);
                }
            }
        }

        protected abstract int NextMuseumCoinOffer();

        /// <summary>
        /// Probability of offering a museum coin (between 0 and 1).
        /// </summary>
        public abstract double ChanceToOfferCoin { get; }

        public abstract void ResetCoinOffers();
    }
}
