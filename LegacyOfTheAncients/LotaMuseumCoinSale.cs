using AgateLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xle.Menus;
using Xle.Menus.Implementation;

namespace Xle.Ancients
{
    [Singleton]
    public class LotaMuseumCoinSale : MuseumCoinSale
    {
        List<int> mMuseumCoinOffers = new List<int>();

        private LotaStory Story { get { return GameState.Story(); } }

        protected override int NextMuseumCoinOffer()
        {
            if (mMuseumCoinOffers.Count == 0)
                return -1;

            int next = mMuseumCoinOffers[0];

            mMuseumCoinOffers.RemoveAt(0);

            return next;
        }

        public override double ChanceToOfferCoin
        {
            get { return 0.045; }
        }

        public override void ResetCoinOffers()
        {
            mMuseumCoinOffers.Clear();

            switch (GameState.Player.Level)
            {
                case 1:
                case 2:
                    if (Story.ReturnedTulip == false)
                    {
                        AddCoins(0, LotaItem.JadeCoin, LotaItem.TopazCoin);
                    }
                    else
                    {
                        AddCoins(0, LotaItem.TopazCoin, LotaItem.JadeCoin);
                    }
                    for (int i = 0; i < mMuseumCoinOffers.Count; i++)
                    {
                        if (GameState.Player.Items[mMuseumCoinOffers[i]] >= 1)
                        {
                            mMuseumCoinOffers.RemoveAt(i);
                            i--;
                        }
                    }
                    break;

                case 3:
                    if (Story.ReturnedTulip == false)
                    {
                        AddCoins(0, LotaItem.JadeCoin, LotaItem.AmethystCoin, LotaItem.TopazCoin);
                    }
                    else if (Story.PirateComplete == false)
                    {
                        AddCoins(0, LotaItem.TopazCoin, LotaItem.AmethystCoin, LotaItem.JadeCoin);
                    }
                    else
                    {
                        if (GameState.Player.Items[LotaItem.AmethystCoin] == 0)
                            AddCoins(0, LotaItem.AmethystCoin);

                        AddCoins(0.5, LotaItem.TopazCoin, LotaItem.JadeCoin);
                    }
                    for (int i = 0; i < mMuseumCoinOffers.Count; i++)
                    {
                        if (GameState.Player.Items[mMuseumCoinOffers[i]] >= 1)
                        {
                            mMuseumCoinOffers.RemoveAt(i);
                            i--;
                        }
                    }

                    break;

                case 4:
                case 5:
                    if (Story.ArmakComplete == false && GameState.Player.Items[LotaItem.SapphireCoin] == 0)
                    {
                        AddCoins(0, LotaItem.SapphireCoin);

                        AddCoins(0.5, LotaItem.AmethystCoin, LotaItem.TopazCoin, LotaItem.JadeCoin);
                    }
                    else
                    {
                        AddCoins(0.5, LotaItem.SapphireCoin, LotaItem.AmethystCoin, LotaItem.TopazCoin, LotaItem.JadeCoin);
                    }
                    break;

                case 6:
                    if (Story.FourJewelsComplete == false && GameState.Player.Items[LotaItem.RubyCoin] == 0)
                    {
                        AddCoins(0, LotaItem.RubyCoin);
                    }

                    AddCoins(0.5, LotaItem.SapphireCoin, LotaItem.AmethystCoin, LotaItem.TopazCoin, LotaItem.JadeCoin);

                    break;

                case 7:
                    if (Story.FortressComplete == false && GameState.Player.Items[LotaItem.DiamondCoin] == 0)
                    {
                        AddCoins(0, LotaItem.DiamondCoin);
                    }

                    AddCoins(0.5, LotaItem.SapphireCoin, LotaItem.AmethystCoin, LotaItem.TopazCoin, LotaItem.JadeCoin);

                    break;
            }
        }

        private T ChooseRandom<T>(params T[] items)
        {
            return items[Random.Next(items.Length)];
        }

        private void AddCoins(double skipChance, params LotaItem[] items)
        {
            foreach (var it in items)
            {
                if (Random.NextDouble() < skipChance)
                    continue;

                if (GameState.Player.Items[it] < 2)
                    mMuseumCoinOffers.Add((int)it);
            }
        }
	
    }
}
