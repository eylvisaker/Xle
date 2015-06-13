using AgateLib.Geometry;
using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.MapExtenders.Museum.MuseumDisplays
{
	public abstract class LotaExhibit : Exhibit
	{
		protected LotaExhibit(string name, Coin c)
			: base(name)
		{
			Coin = c;
		}

		protected int StoryVariable
		{
			get { return Lota.Story.Museum[ExhibitID]; }
			set { Lota.Story.Museum[ExhibitID] = value; }
		}
		
		public Coin Coin { get; private set; }

		public override bool RequiresCoin(Player player)
		{ 
			return Coin != Coin.None; 
		}
		
		protected override void MarkAsVisited(Player player)
		{
			if (StoryVariable == 0)
				StoryVariable = 1;
		}
		public override bool HasBeenVisited(Player player)
		{
			return StoryVariable != 0;
		}
		public bool HasBeenVisited(Player player, ExhibitIdentifier exhibit)
		{
            return Lota.Story.Museum[(int)exhibit] != 0;
		}		
		public override string InsertCoinText
		{
			get { return "(Insert " + Coin.ToString() + " coin)"; }
		}
		public override Color ExhibitColor
		{
			get
			{
				switch (Coin)
				{
					case Coin.Jade: return XleColor.Green;
					case Coin.Topaz: return XleColor.Yellow;
					case Coin.Amethyst: return XleColor.Purple;
					case Coin.Sapphire: return XleColor.Blue;
					case Coin.Ruby: return XleColor.Red;
					case Coin.Turquoise: return XleColor.Cyan;

					case Coin.Diamond:
					case Coin.None:
					default:
						return XleColor.White;
				}
			}
		}
		public static LotaItem ItemFromCoin(Coin coin)
		{
			switch (coin)
			{
				case Coin.Jade: return LotaItem.JadeCoin;
				case Coin.Topaz: return LotaItem.TopazCoin;
				case Coin.Amethyst: return LotaItem.AmethystCoin;
				case Coin.Sapphire: return LotaItem.SapphireCoin;
				case Coin.Ruby: return LotaItem.RubyCoin;
				case Coin.Turquoise: return LotaItem.TurquoiseCoin;
				case Coin.Diamond: return LotaItem.DiamondCoin;
				case Coin.None:
				default:
					return 0;
			}
		}

		public abstract ExhibitIdentifier ExhibitIdentifier { get; }
		
		public override int ExhibitID
		{
			get
			{
				return (int)ExhibitIdentifier;
			}
		}

		public ExhibitInfo ExhibitInfo
		{
			get { return XleCore.Data.ExhibitInfo[(int)ExhibitID]; }
		}

		public override void Draw(Rectangle displayRect)
		{
			ExhibitInfo.DrawImage(displayRect, ImageID);
		}

		public override string UseCoinMessage
		{
			get
			{
				return "insert your " + Coin.ToString() + " coin?";
			}
		}

		public override bool PlayerHasCoin(Player player)
		{
			return player.Items[ItemFromCoin(Coin)] > 0;
		}

		public override void UseCoin(Player player)
		{
			if (player.Items[ItemFromCoin(Coin)] <= 0)
				throw new InvalidOperationException("Cannot use a coin the player does not have!");

			player.Items[ItemFromCoin(Coin)]--;
		}
	}
}
