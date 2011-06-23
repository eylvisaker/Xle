using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.MuseumDisplays
{
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

		public override void PlayerXamine(Player player)
		{
			if (player.museum[ExhibitID] == 0 && TotalExhibitsViewed(player) < 2)
			{
				ReadRawText(XleCore.ExhibitInfo[ExhibitID].Text[1]);
				
				// fair knife
				player.AddWeapon(1, 1);
			}
			else if (player.museum[ExhibitID] == 1)
			{
				ReadRawText(XleCore.ExhibitInfo[ExhibitID].Text[2]);

				// great bladed staff
				player.AddWeapon(3, 3);

				player.museum[ExhibitID] = 10;
			}
		}
		public override bool IsClosed(ERY.Xle.Player player)
		{
			if (player.museum[ExhibitID] == 10)
				return true;

			if (player.museum[ExhibitID] == 1 && TotalExhibitsViewed(player) < 2)
				return true;

			return false;
		}
	}
	
}
