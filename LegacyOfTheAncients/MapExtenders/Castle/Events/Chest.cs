using ERY.Xle.XleEventTypes.Extenders;

namespace ERY.Xle.LotA.MapExtenders.Castle.Events
{
	class Chest : TreasureChestExtender
	{
		public override void BeforeGiveItem(GameState state, ref int item, ref int count)
		{
			if (item == (int)LotaItem.CopperKey && state.Player.Items[item] > 0)
				item++;
		}

		public int CastleLevel { get; set; }

		public override void MarkChestAsOpen(GameState state)
		{
			var chests = ChestArray(state);

			chests[TheEvent.ChestID] = 1;
		}
		public override void OpenIfMarked(GameState state)
		{
			var chests = ChestArray(state);

			if (chests[TheEvent.ChestID] != 0)
				TheEvent.SetOpenTilesOnMap(state.Map);
		}

		private int[] ChestArray(GameState state)
		{
			var chests = CastleLevel == 1 ? Lota.Story.CastleGroundChests : Lota.Story.CastleUpperChests;

			return chests;
		}
	}
}
