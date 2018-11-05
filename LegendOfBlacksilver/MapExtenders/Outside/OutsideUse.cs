using AgateLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xle;

namespace Xle.Blacksilver.MapExtenders.Outside
{
    [Transient("OutsideUse")]
    public class OutsideUse : LobUse
    {
        protected override async Task<bool> UseWithMap(int item)
        {
            switch ((LobItem)item)
            {
                case LobItem.ClimbingGear:
                    return true;

                default:
                    return false;
            }
        }
    }
}
