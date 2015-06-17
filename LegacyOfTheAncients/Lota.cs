using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Services.Implementation;

namespace ERY.Xle.LotA
{
	static class Lota
	{
		static List<int> mMuseumCoinOffers = new List<int>();

		public static LotaStory Story { get { return XleCore.GameState.Story(); } }

		public static int NextMuseumCoinOffer()
		{
			if (mMuseumCoinOffers.Count == 0)
				return -1;

			int next = mMuseumCoinOffers[0];

			mMuseumCoinOffers.RemoveAt(0);

			return next;
		}

		public static void SetMuseumCoinOffers(GameState state)
		{
			mMuseumCoinOffers.Clear();

			switch (state.Player.Level)
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
						if (state.Player.Items[mMuseumCoinOffers[i]] >= 1)
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
						if (state.Player.Items[LotaItem.AmethystCoin] == 0)
							AddCoins(0, LotaItem.AmethystCoin);

						AddCoins(0.5, LotaItem.TopazCoin, LotaItem.JadeCoin);
					}
					for (int i = 0; i < mMuseumCoinOffers.Count; i++)
					{
						if (state.Player.Items[mMuseumCoinOffers[i]] >= 1)
						{
							mMuseumCoinOffers.RemoveAt(i);
							i--;
						}
					}
					
					break;

				case 4:
				case 5:
					if (Story.ArmakComplete == false && state.Player.Items[LotaItem.SapphireCoin] == 0)
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
					if (Story.FourJewelsComplete == false && state.Player.Items[LotaItem.RubyCoin] == 0)
					{
						AddCoins(0, LotaItem.RubyCoin);
					}
					
					AddCoins(0.5, LotaItem.SapphireCoin, LotaItem.AmethystCoin, LotaItem.TopazCoin, LotaItem.JadeCoin);

					break;

				case 7:
					if (Story.FortressComplete == false && state.Player.Items[LotaItem.DiamondCoin] == 0)
					{
						AddCoins(0, LotaItem.DiamondCoin);
					}
					
					AddCoins(0.5, LotaItem.SapphireCoin, LotaItem.AmethystCoin, LotaItem.TopazCoin, LotaItem.JadeCoin);

					break;
			}
		}

		private static T ChooseRandom<T>(params T[] items)
		{
			return items[XleCore.random.Next(items.Length)];
		}

		private static void AddCoins(double skipChance, params LotaItem[] items)
		{
			foreach (var it in items)
			{
				if (XleCore.random.NextDouble() < skipChance)
					continue;

				if (XleCore.GameState.Player.Items[it] < 2)
					mMuseumCoinOffers.Add((int)it);
			}
		}
	}
}
