using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.MapExtenders.Dungeons
{
	class PirateExtender : LotaDungeonExtenderBase
	{
		protected override bool IsComplete(ERY.Xle.Player player)
		{
			return player.Story().PirateComplete;
		}
		protected override void SetComplete(Player player)
		{
			player.Story().PirateComplete = true;
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
