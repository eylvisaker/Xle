using AgateLib.Geometry;
using ERY.Xle.XleMapTypes.MuseumDisplays;
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
		public override bool RequiresCoin(GameState state)
		{ 
			return Coin != Coin.None; 
		} 
		
		string CoinName
		{
			get
			{
				switch(Coin)
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
				
		public override string CoinString
		{
			get { return "(Insert " + CoinName + ")"; }
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
			get { return XleCore.ExhibitInfo[(int)ExhibitID]; }
		}
		
		public override bool ViewedBefore(Player player)
		{
			return player.museum[(int)ExhibitID] != 0;
		}
	
		public override void Draw(Rectangle displayRect)
		{
			ExhibitInfo.DrawImage(displayRect, ImageID);
		}
		protected int TotalExhibitsViewed(Player player)
		{
			int count = 0;

			for (int i = 2; i < player.museum.Length; i++)
			{
				if (player.museum[i] != 0) 
					count++;
			}

			return count;
		}
	}
}
