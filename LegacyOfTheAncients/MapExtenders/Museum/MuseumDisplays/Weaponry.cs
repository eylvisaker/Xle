using ERY.Xle.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.MapExtenders.Museum.MuseumDisplays
{
	class Weaponry : Exhibit
	{
		public Weaponry() : base("Weaponry", Coin.Jade) { }
		public override ExhibitIdentifier ExhibitID { get { return ExhibitIdentifier.Weaponry; } }
		public override string LongName
		{
			get
			{
				return "The ancient art of weaponry";
			}
		}

		bool viewedThisTime;

		public override void PlayerXamine(Player player)
		{
			int id = (int)ExhibitID;

			if (player.museum[(int)ExhibitIdentifier.Thornberry] != 0)
			{
				player.museum[id] = 1;
			}

			if (player.museum[id] == 0)
			{
				ReadRawText(ExhibitInfo.Text[1]);
				
				// fair knife
				player.AddWeapon(1, 1);
			}
			else if (player.museum[id] == 1)
			{
				ReadRawText(ExhibitInfo.Text[2]);

				// great bladed staff
				player.AddWeapon(3, 3);

				player.museum[id] = 10;
			}

			viewedThisTime = true;
		}
		public override bool IsClosed(ERY.Xle.Player player)
		{
			int id = (int)ExhibitID;

			if (viewedThisTime)
				return true;

			if (player.museum[id] == 10)
				return true;

			return false;
		}
	}
	
}
