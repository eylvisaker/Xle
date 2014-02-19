using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.Extenders.Dungeons
{
	class FourJewelsExtender : LotaDungeonExtenderBase
	{
		protected override string CompleteVariable
		{
			get { return "FourJewelComplete"; }
		}
		protected override int StrengthBoost
		{
			get { return 10; }
		}
		public override void OnBeforeGiveItem(Player player, ref int treasure, ref bool handled)
		{
			if (treasure == (int)LotaItem.GuardJewel)
			{
				if (player.Items[LotaItem.GuardJewel] >= 4)
					treasure = 0;
			}
		}
	}
}
