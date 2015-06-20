using ERY.Xle.XleEventTypes.Extenders;

namespace ERY.Xle.LotA.MapExtenders.Castle.Events
{
    public class TreasureChest : TreasureChestExtender
    {
        public LotaStory Story
        {
            get { return GameState.Story(); }
        }

        public override void BeforeGiveItem(GameState state, ref int item, ref int count)
        {
            if (item == (int)LotaItem.CopperKey && Player.Items[item] > 0)
                item++;
        }

        public int CastleLevel
        {
            get
            {
                return GameState.Map.ExtenderName.Contains("CastleGround") ? 1 : 2;
            }
        }

        public override void MarkChestAsOpen(GameState state)
        {
            var chests = ChestArray(GameState);

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
            var chests = CastleLevel == 1 ? Story.CastleGroundChests : Story.CastleUpperChests;

            return chests;
        }
    }
}
