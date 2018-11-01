using AgateLib;
using Xle.XleEventTypes.Extenders;

namespace Xle.Blacksilver.MapExtenders.Citadel.EventExtenders
{
    [Transient("CitadelDoor")]
    public class CitadelDoor : DoorExtender
    {
        public override bool ItemUnlocksDoor(int item)
        {
            if (item == (int)LobItem.QuartzKey)
            {
                return true;
            }
            else
            {
                return base.ItemUnlocksDoor(item);
            }
        }
    }
}
