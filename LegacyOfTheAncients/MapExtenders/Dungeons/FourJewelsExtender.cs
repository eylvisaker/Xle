using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.MapExtenders.Dungeons
{
	class FourJewelsExtender : LotaDungeonExtenderBase
	{
		protected override bool IsComplete(Player player)
		{
			return player.Story().FourJewelsComplete;
		}
		protected override void SetComplete(Player player)
		{
			player.Story().FourJewelsComplete = true;
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
