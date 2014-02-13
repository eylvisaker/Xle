using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;

namespace ERY.Xle.XleMapTypes.MuseumDisplays
{
	public enum Coin
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
	public enum ExhibitIdentifier
	{
		Information,
		Welcome,
		Weaponry,
		Thornberry,
		Fountain,
		PirateTreasure,
		HerbOfLife,
		NativeCurrency,
		StonesWisdom,
		Tapestry,
		LostDisplays,
		KnightsTest,
		FourJewels,
		Guardian,
		Pegasus,
		AncientArtifact,
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
		/// <summary>
		/// Gets the color of the text in the museum. Defaults to ExhibitColor.
		/// </summary>
		public virtual Color TextColor
		{
			get { return ExhibitColor; }
		}
		protected virtual string RawText
		{
			get
			{
				if (XleCore.ExhibitInfo.ContainsKey((int)ExhibitID))
				{
					var exinfo = XleCore.ExhibitInfo[(int)ExhibitID];

					if (exinfo.Text.ContainsKey(1))
						return XleCore.ExhibitInfo[(int)ExhibitID].Text[1];
					else
						return "This exhibit does not have any text with key 1.";
				}
				else
				{
					return "This exhibit is not working.";
				}
			}
				
		}

		protected int ImageID { get; set; }

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
			if (player.museum[(int)ExhibitID] > 0)
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
			XleCore.BottomTextMargin = 0;
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

					int i = 1;
					while (rawtext[ip + i] == ' ')
						i++;

					if (rawtext[ip + i] == '|')
						ip += i - 1;
				}
				else if (rawtext[ip] == '|' && (text.Text == null || text.Text.Trim() == ""))
				{
					text.Clear();
				}
				else if (rawtext[ip] != '`')
				{
					text.AddText(rawtext[ip].ToString(), clr);
					g.UpdateBottom(text, line);

					if (waiting)
					{
						if (AgateLib.InputLib.Keyboard.AnyKeyPressed)
						{
							XleCore.wait(1);
						}
						else
							XleCore.wait(50);
					}
				}
				else
				{
					int next = rawtext.IndexOf('`', ip + 1);
					if (next < 0)
						throw new ArgumentException("Text had unmatched quote!");

					string substr = rawtext.Substring(ip + 1, next - ip - 1);

					ip = next;

					if (substr.StartsWith("image:"))
					{
						int image = int.Parse(substr.Substring(6));

						ImageID = image;
					}
					else
					{
						switch (substr)
						{
							case "": break;
							case "white": clr = XleColor.White; break;
							case "cyan": clr = XleColor.Cyan; break;
							case "yellow": clr = XleColor.Yellow; break;
							case "green": clr = XleColor.Green; break;
							case "purple": clr = XleColor.Purple; break;

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
				}

				ip++;
			}

			XleCore.BottomTextMargin = 1;
		}

		public virtual bool IsClosed(Player player)
		{
			return false;
		}

		public abstract ExhibitIdentifier ExhibitID { get; }
		public virtual bool ViewedBefore(Player player)
		{
			return player.museum[(int)ExhibitID] != 0;
		}

		/// <summary>
		/// Returns true if we should draw the static before a coin is inserted.
		/// </summary>
		public virtual bool StaticBeforeCoin
		{
			get { return true; }
		}

		public ExhibitInfo ExhibitInfo
		{
			get { return XleCore.ExhibitInfo[(int)ExhibitID]; }
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

		public virtual void Draw(Rectangle displayRect)
		{
			ExhibitInfo.DrawImage(displayRect, ImageID);
		}
	}



}