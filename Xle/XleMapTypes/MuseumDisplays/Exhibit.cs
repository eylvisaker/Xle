using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;

namespace ERY.Xle.XleMapTypes.MuseumDisplays
{
	enum Coin
	{
		None,
		Jade,
		Topaz,
		Amethyst,
		Sapphire,
		Ruby,
		Turquoise,
		Diamond,
	}
	abstract class Exhibit
	{
		protected Exhibit(string name, Coin c)
		{
			Name = name;
			Coin = c;
		}

		public string Name { get; private set; }
		public Coin Coin { get; private set; }
		public bool RequiresCoin { get { return Coin != Coin.None; } }
		public virtual string LongName { get { return Name; } }
		public virtual string CoinString
		{
			get { return "(Insert " + Coin.ToString() + " coin)"; }
		}
		public virtual Color ExhibitColor
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

		public virtual void PlayerXamine(Player player)
		{

		}
		public virtual bool IsClosed(Player player)
		{
			return false;
		}

		public abstract int ExhibitID { get; }
		public virtual bool ViewedBefore(Player player)
		{
			return player.museum[ExhibitID] != 0;
		}
	}

	class Information : Exhibit
	{
		public Information() : base("Information", Coin.None) { }
		public override int ExhibitID { get { return 0; } }
		public override string CoinString
		{
			get { return string.Empty; }
		}
	}
	class Welcome : Exhibit
	{
		public Welcome() : base("Welcome", Coin.None) { }
		public override int ExhibitID { get { return 1; } }
		public override string LongName
		{
			get { return "Welcome to the famed"; }
		}
		public override string CoinString
		{
			get { return "Tarmalon Museum!"; }
		}

		public override void PlayerXamine(Player player)
		{

		}
		
	}

	class Weaponry : Exhibit
	{
		public Weaponry() : base("Weaponry", Coin.Jade) { }
		public override int ExhibitID { get { return 2; } }
		public override string LongName
		{
			get
			{
				return "The ancient art of weaponry";
			}
		}
	}
	class Thornberry : Exhibit
	{
		public Thornberry() : base("Thornberry", Coin.Jade) { }
		public override int ExhibitID { get { return 3; } }
		public override string LongName
		{
			get { return "A typical town of Tarmalon"; }
		}
	}
	class Fountain : Exhibit
	{
		public Fountain() : base("A Fountain", Coin.Jade) { }
		public override int ExhibitID { get { return 4; } }
		public override string LongName
		{
			get
			{
				return "Enchanted flower fountain";
			}
		}
	}
	class PirateTreasure : Exhibit
	{
		public PirateTreasure() : base("Pirate Treasure", Coin.Topaz) { }
		public override int ExhibitID { get { return 5; } }
	}
	class HerbOfLife : Exhibit
	{
		public HerbOfLife() : base("Herb of life", Coin.Topaz) { }
		public override int ExhibitID { get { return 6; } }
		public override string LongName
		{
			get
			{
				return "The herb of life";
			}
		}
	}
	class NativeCurrency : Exhibit
	{
		public NativeCurrency() : base("Native Currency", Coin.Topaz) { }
		public override int ExhibitID { get { return 7; } }
	}
	class StonesWisdom : Exhibit
	{
		public StonesWisdom() : base("Stones of Wisdom", Coin.Amethyst) { }
		public override int ExhibitID { get { return 8; } }
	}
	class Tapestry : Exhibit
	{
		public Tapestry() : base("A Tapestry", Coin.Amethyst) { }
		public override int ExhibitID { get { return 9; } }
	}
	class LostDisplays : Exhibit
	{
		public LostDisplays() : base("Lost Displays", Coin.Sapphire) { }
		public override int ExhibitID { get { return 10; } }
		public override string LongName
		{
			get
			{
				return "The lost displays";
			}
		}
	}
	class KnightsTest : Exhibit
	{
		public KnightsTest() : base("KnightsTest", Coin.Sapphire) { }
		public override int ExhibitID { get { return 11; } }
	}
	class FourJewels : Exhibit
	{
		public FourJewels() : base("FourJewels", Coin.Ruby) { }
		public override int ExhibitID { get { return 12; } }
	}
	class Guardian : Exhibit
	{
		public Guardian() : base("Guardian", Coin.Turquoise) { }
		public override int ExhibitID { get { return 13; } }
	}
	class Pegasus : Exhibit
	{
		public Pegasus() : base("Pegasus", Coin.Diamond) { }
		public override int ExhibitID { get { return 14; } }
	}
	class AncientArtifact : Exhibit
	{
		public AncientArtifact() : base("Ancient Artifact", Coin.None) { }
		public override int ExhibitID { get { return 15; } }
		public override string LongName
		{
			get
			{
				return "An ancient artifact";
			}
		}
		public override string CoinString
		{
			get { return string.Empty; }
		}
		public override bool IsClosed(Player player)
		{
			return true;
		}
		public override Color ExhibitColor
		{
			get { return XleColor.Cyan; }
		}
	}
}