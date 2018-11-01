using AgateLib;
using Xle.XleEventTypes.Extenders;

namespace Xle.Blacksilver.MapExtenders.Castle.EventExtenders
{
    [Transient("TreasureChest")]
    public class TreasureChest : TreasureChestExtender
    {
        protected LobStory Story { get { return GameState.Story(); } }

        public override void SetAngry()
        {
        }

        public override void MarkChestAsOpen()
        {
            Story.CastleChests[ChestArrayIndex][TheEvent.ChestID] = 1;
        }

        public override void OpenIfMarked()
        {
            if (Story.CastleChests[ChestArrayIndex][TheEvent.ChestID] == 1)
            {
                TheEvent.SetOpenTilesOnMap(Map);
            }
        }

        public int ChestArrayIndex { get; set; }

        public override string TakeFailMessage
        {
            get
            {
                return "Nothing to take.";
            }
        }
    }
}
