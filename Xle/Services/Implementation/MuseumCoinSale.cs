using ERY.Xle.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.Services.Implementation
{
    public class MuseumCoinSale : IMuseumCoinSale
    {
        public Random Random { get; set; }
        public ITextArea TextArea { get; set; }
        public IXleGameControl GameControl { get; set; }
        public XleSystemState systemState { get; set; }
        public GameState GameState { get; set; }
        public ISoundMan SoundMan { get; set; }
        public IQuickMenu QuickMenu { get; set; }
        public XleData Data { get; set; }

        public Player Player { get { return GameState.Player; } }

        public void OfferMuseumCoin()
        {
            int coin = -1;
            MenuItemList menu = new MenuItemList("Yes", "No");

            coin = systemState.Factory.NextMuseumCoinOffer(GameState);

            if (coin == -1)
                return;

            // TODO: only allow player to buy a coin if he has less than Level of that type of coins.
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

    }
}
