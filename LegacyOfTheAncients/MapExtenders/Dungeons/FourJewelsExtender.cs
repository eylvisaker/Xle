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
			return Lota.Story.FourJewelsComplete;
		}
		protected override void SetComplete(Player player)
		{
			Lota.Story.FourJewelsComplete = true;
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

		public override int GetTreasure(GameState state, int dungeonLevel, int chestID)
		{
			if (state.Player.Items[LotaItem.GuardJewel] >= 4)
				return 0;

			return (int)LotaItem.GuardJewel;
		}
	}
}
