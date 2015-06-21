using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Labyrinth.EventExtenders
{
    public class LabyrinthDoor : DoorExtender
    {
        public override bool ItemUnlocksDoor(GameState state, int item)
        {
            if (item == (int)LobItem.SkeletonKey)
            {
                return true;
            }

            return base.ItemUnlocksDoor(state, item);
        }
    }
}
