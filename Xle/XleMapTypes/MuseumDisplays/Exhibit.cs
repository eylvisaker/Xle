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
		protected virtual string RawText
		{
			get
			{
				if (XleCore.ExhibitInfo.ContainsKey(ExhibitID))
				{
					var exinfo = XleCore.ExhibitInfo[ExhibitID];

					if (exinfo.Text.ContainsKey(1))
						return XleCore.ExhibitInfo[ExhibitID].Text[1];
					else
						return "This exhibit does not have any text with key 1.";
				}
				else
				{
					return "This exhibit is not working.";
				}
			}
				
		}

		public virtual void PlayerXamine(Player player)
		{
			if (CheckOfferReread(player) == false)
				return;

			ReadRawText(RawText);
		}

		/// <summary>
		/// Returns true if we are reading the exhibit for the first time,
		/// or if the player answered yes to rereading the exhibit.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		protected bool CheckOfferReread(Player player)
		{
			if (player.museum[ExhibitID] > 0)
			{
				return OfferReread();
			}

			return true;
		}

		/// <summary>
		/// Asks the player if they want to reread the description of the exhibit.
		/// </summary>
		/// <returns>True if the player chose yes, false otherwise.</returns>
		protected bool OfferReread()
		{
			g.ClearBottom();
			g.AddBottom("Do you want to reread the");
			g.AddBottom("description of this exhibit?");
			g.AddBottom();

			if (XleCore.QuickMenu(new MenuItemList("Yes", "No"), 3) == 0)
				return true;
			else
				return false;
		}

		protected void ReadRawText(string rawtext)
		{
			g.ClearBottom();

			int ip = 0;
			int line = 4;
			Color clr = XleColor.Cyan;
			ColorStringBuilder text = new ColorStringBuilder();
			bool waiting = true;

			while (ip < rawtext.Length)
			{
				if (rawtext[ip] == '\r') { ip++; continue; }

				if (rawtext[ip] == '\n')
				{
					line--;
					text = new ColorStringBuilder();
				}
				else if (rawtext[ip] != '`')
				{
					text.AddText(rawtext[ip].ToString(), clr);
					g.UpdateBottom(text, line);
					
					if (waiting)
						XleCore.wait(50);
				}
				else
				{
					int next = rawtext.IndexOf('`', ip + 1);
					if (next < 0)
						throw new ArgumentException("Text had unmatched quote!");

					string substr = rawtext.Substring(ip+1, next - ip-1);

					ip = next;

					switch (substr)
					{
						case "white": clr = XleColor.White; break;
						case "cyan": clr = XleColor.Cyan; break;
						case "yellow": clr = XleColor.Yellow; break;
						case "pause":
							XleCore.WaitForKey();

							break;

						case "clear":
							g.ClearBottom();
							line = 4;
							break;

						case "sound:VeryGood":
							SoundMan.PlaySound(LotaSound.VeryGood);
							break;

						case "wait:off":
							waiting = false;
							break;

						case "wait:on":
							waiting = true;
							break;

						default:
							System.Diagnostics.Trace.WriteLine("Failed to understand command: " + substr);
							break;
					}
				}

				ip++;
			}
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

	abstract class ExhibitData
	{
		public abstract void Execute();
	}
	class ExhibitText : ExhibitData
	{
		ColorStringBuilder csb;

		public ExhibitText(ColorStringBuilder csb)
		{
			this.csb = csb;
		}

		public override void Execute()
		{
			g.AddBottom(csb);
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
		public override string LongName
		{
			get { return "A flight of fancy"; }
		}
	}
	class AncientArtifact : Exhibit
	{
		public AncientArtifact() : base("Ancient Artifact", Coin.None) { }
		public override int ExhibitID { get { return 15; } }
		public override string LongName
		{
			get { return "An ancient artifact"; }
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