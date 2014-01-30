using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.Extenders
{
	class PirateExtender : LotaDungeonExtenderBase
	{
		protected override string CompleteVariable
		{
			get { return "PirateComplete"; }
		}
		protected override int StrengthBoost
		{
			get { return 10; }
		}
		public override void OnBeforeGiveItem(Player player, ref int treasure, ref bool handled)
		{
			// crown
			if (treasure == (int)LotaItem.Crown)
			{
				if (player.Items[LotaItem.Crown] > 0)
					treasure = 0;

				if (player.Level > 3)
					treasure = 0;
			}
		}
	}
}
