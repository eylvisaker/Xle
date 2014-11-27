using AgateLib.Geometry;
using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Archives.Exhibits
{
	public abstract class LobExhibit : Exhibit
	{
		protected LobExhibit(string name, Coin c)
			: base(name)
		{
			Coin = c;
		}

		public Coin Coin { get; private set; }

		string CoinName
		{
			get
			{
				switch (Coin)
				{
					case Exhibits.Coin.BlueGem: return "Blue gem";
					case Exhibits.Coin.RedGarnet: return "red garnet";
					case Exhibits.Coin.AmethystGem: return "amethyst gem";
					case Exhibits.Coin.Emerald: return "emerald";
					case Exhibits.Coin.YellowDiamond: return "yellow diamond";
					case Exhibits.Coin.WhiteDiamond: return "white diamond";
					case Exhibits.Coin.BlackOpal: return "black opal";
					default:
						throw new ArgumentException();
				}
			}
		}

		public static LobItem ItemFromCoin(Coin coin)
		{
			switch (coin)
			{
				case Coin.AmethystGem: return LobItem.AmethystGem;
				case Coin.BlackOpal: return LobItem.Opal;
				case Coin.BlueGem: return LobItem.BlueGem;
				case Coin.Emerald: return LobItem.Emerald;
				case Coin.RedGarnet: return LobItem.RedGarnet;
				case Coin.WhiteDiamond: return LobItem.WhiteDiamond;
				case Coin.YellowDiamond: return LobItem.YellowDiamond;

				case Coin.None:
				default:
					return 0;
			}
		}
		protected void ReturnGem(Player player)
		{
			player.Items[ItemFromCoin(Coin)]++;

			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("We're returning your gem.");
		}

		protected override Color ArticleTextColor
		{
			get { return XleColor.White; }
		}
		protected override int TextAreaMargin
		{
			get { return 1; }
		}

		public override string InsertCoinText
		{
			get { return ""; }
		}
		public override Color ExhibitColor
		{
			get
			{
				switch (Coin)
				{
					case Coin.BlueGem: return XleColor.Cyan;
					case Coin.RedGarnet: return XleColor.Red;
					case Coin.AmethystGem: return XleColor.Purple;
					case Coin.Emerald: return XleColor.Green;
					case Coin.YellowDiamond: return XleColor.Yellow;
					case Coin.WhiteDiamond: return XleColor.White;
					case Coin.BlackOpal: return XleColor.Blue;

					case Coin.None:
					default:
						return XleColor.White;
				}
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

		protected override void MarkAsVisited(Player player)
		{
			Lob.Story.VisitedArchive[ExhibitID] = 1;
		}
		public override bool HasBeenVisited(Player player)
		{
			return Lob.Story.VisitedArchive[ExhibitID] != 0;
		}

		public override bool RequiresCoin(Player player)
		{
			return Coin != Coin.None;
		}

		public override string IntroductionText
		{
			get
			{
				return "A sign says...";
			}
		}
		public override string UseCoinMessage
		{
			get
			{
				return "cost:  one " + CoinName;
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
