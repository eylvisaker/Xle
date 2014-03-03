using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.MapExtenders.Museum.MuseumDisplays
{
	class Weaponry : LotaExhibit
	{
		public Weaponry() : base("Weaponry", Coin.Jade) { }
		public override ExhibitIdentifier ExhibitIdentifier { get { return ExhibitIdentifier.Weaponry; } }
		public override string LongName
		{
			get
			{
				return "The ancient art of weaponry";
			}
		}

		bool viewedThisTime;

		public override void RunExhibit(Player player)
		{
			int id = (int)ExhibitIdentifier;

			if (player.Story().Museum[(int)ExhibitIdentifier.Thornberry] != 0)
			{
				player.Story().Museum[id] = 1;
			}

			if (player.Story().Museum[id] == 0)
			{
				ReadRawText(ExhibitInfo.Text[1]);
				
				// fair knife
				player.AddWeapon(1, 1);
			}
			else if (player.Story().Museum[id] == 1)
			{
				ReadRawText(ExhibitInfo.Text[2]);

				// great bladed staff
				player.AddWeapon(3, 3);

				player.Story().Museum[id] = 10;
			}

			viewedThisTime = true;
		}
		public override bool IsClosed(ERY.Xle.Player player)
		{
			int id = (int)ExhibitIdentifier;

			if (viewedThisTime)
				return true;

			if (player.Story().Museum[id] == 10)
				return true;

			return false;
		}
	}
	
}
