using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib;
using AgateLib.Mathematics.Geometry;
using ERY.Xle.Data;
using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;
using ERY.Xle.Services;

namespace ERY.Xle.LotA.MapExtenders.Museum.MuseumDisplays
{
	public abstract class LotaExhibit : Exhibit
	{
		protected LotaExhibit(string name, Coin c)
			: base(name)
		{
			Coin = c;
		}


		protected LotaStory Story
		{
			get { return GameState.Story(); }
		}

		protected int StoryVariable
		{
			get { return Story.Museum[ExhibitID]; }
			set { Story.Museum[ExhibitID] = value; }
		}

		public Coin Coin { get; private set; }

		public override bool RequiresCoin
		{
			get { return Coin != Coin.None; }
		}

		protected override void MarkAsVisited()
		{
			if (StoryVariable == 0)
				StoryVariable = 1;
		}

		public override bool HasBeenVisited
		{
			get { return StoryVariable != 0; }
		}

		public bool ExhibitHasBeenVisited(ExhibitIdentifier exhibit)
		{
			return Story.Museum[(int)exhibit] != 0;
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
			get { return Data.ExhibitInfo[(int)ExhibitID]; }
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

		public override bool PlayerHasCoin
		{
			get { return Player.Items[ItemFromCoin(Coin)] > 0; }
		}

		public override void UseCoin()
		{
			if (Player.Items[ItemFromCoin(Coin)] <= 0)
				throw new InvalidOperationException("Cannot use a coin the player does not have!");

			Player.Items[ItemFromCoin(Coin)]--;
		}
	}
}
