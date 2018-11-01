using Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib;

namespace Xle.Blacksilver.MapExtenders.Labyrinth.EventExtenders
{
    [Transient("LabyrinthDoor")]
    public class LabyrinthDoor : DoorExtender
    {
        public override bool ItemUnlocksDoor(int item)
        {
            if (item == (int)LobItem.SkeletonKey)
            {
                return true;
            }

            return base.ItemUnlocksDoor(item);
        }
    }
}
