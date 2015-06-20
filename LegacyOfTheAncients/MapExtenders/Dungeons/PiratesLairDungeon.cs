using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.MapExtenders.Dungeons
{
	public class PiratesLairDungeon : LotaDungeon
	{
		protected override bool IsCompleted
		{
			get { return Lota.Story.PirateComplete; }
			set { Lota.Story.PirateComplete = value; }
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
		public override int GetTreasure(GameState state, int dungeonLevel, int chestID)
		{
			if (chestID == 1) return (int)LotaItem.Crown;
			if (chestID == 2) return (int)LotaItem.SapphireCoin;

			return 0;
		}
		public override void OnPlayerExitDungeon(Player player)
		{
			if (IsCompleted)
				return;

			if (player.Items[LotaItem.Crown] > 0 && player.Items[LotaItem.SapphireCoin] > 0)
			{
				IsCompleted = true;

				GivePermanentStrengthBoost(player);
			}
		}

		public override Maps.Map3DSurfaces Surfaces(GameState state)
		{
			return Lota3DSurfaces.DungeonBlue;
		}
	}
}
