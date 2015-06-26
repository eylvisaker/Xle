using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Citadel.EventExtenders
{
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
