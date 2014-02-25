using AgateLib.Geometry;
using ERY.Xle.XleMapTypes.MuseumDisplays;
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
		
		public Coin Coin { get; private set; }
		public override bool RequiresCoin(GameState state)
		{ 
			return Coin != Coin.None; 
		} 
		
				
		public override string CoinString
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
