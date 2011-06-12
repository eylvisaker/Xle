using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.MuseumDisplays
{
	class Information : Exhibit
	{
		public Information() : base("Information", Coin.None) { }
		public override int ExhibitID { get { return 0; } }
		public override string CoinString
		{
			get { return string.Empty; }
		}

		public override void PlayerXamine(Player player)
		{
			bool[] bits = GetBitStatus(player.museum[0]);
			var exhibit = XleCore.ExhibitInfo[ExhibitID];

			if (DoLevelUp(player))
				return;

			if (bits[0] == false)
			{
				// introductory text.
				ReadRawText(exhibit.Text[2]);
			}
			else if (bits[1] == false && player.Item(15) == 0)
			{
				// lost compendium
				ReadRawText(exhibit.Text[3]);

				SetBit(player, 1);
			}
			else if (player.Item(13) > 0 && player.Item(16) > 0)
			{
				// found scepter and crown
				ReadRawText(exhibit.Text[10]);

				// give magic ice.
				player.ItemCount(12, 1);
			}
			else
			{
				ReadRawText(exhibit.Text[1]);
			}
		}

		private bool DoLevelUp(Player player)
		{
			return false;
		}

		private void SetBit(Player player, int p)
		{
			int val = 1 << p;

			player.museum[0] |= val;
		}

		private bool[] GetBitStatus(int value)
		{
			bool[] retval = new bool[32];

			for (int i = 0; i < 32; i++)
			{
				int test = 1 << i;

				retval[i] = (value & test) != 0;
			}

			return retval;
		}
	}
}
