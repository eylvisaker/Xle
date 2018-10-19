using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xle.LoB.MapExtenders.Citadel.EventExtenders
{
    public class StaffPortal : ChangeMapTeleporter
    {
        public override bool StepOn()
        {
            return true;
        }

        public override bool Use(int item)
        {
            if (item != (int)LobItem.Staff)
                return false;

            TeleportAnimation();

            ExecuteMapChange();

            return true;
        }
    }
}
