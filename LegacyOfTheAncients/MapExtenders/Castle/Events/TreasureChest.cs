using Xle.XleEventTypes.Extenders;

namespace Xle.Ancients.MapExtenders.Castle.Events
{
    public class TreasureChest : TreasureChestExtender
    {
        public LotaStory Story
        {
            get { return GameState.Story(); }
        }

        public override void BeforeGiveItem(ref int item, ref int count)
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

        public override void MarkChestAsOpen()
        {
            var chests = ChestArray();

            chests[TheEvent.ChestID] = 1;
        }
        public override void OpenIfMarked()
        {
            var chests = ChestArray();

            if (chests[TheEvent.ChestID] != 0)
                TheEvent.SetOpenTilesOnMap(Map);
        }

        private int[] ChestArray()
        {
            var chests = CastleLevel == 1 ? Story.CastleGroundChests : Story.CastleUpperChests;

            return chests;
        }
    }
}
