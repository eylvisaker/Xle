﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xle.Services;

namespace Xle.LoB.MapExtenders.Outside
{
    [ServiceName("OutsideUse")]
    public class OutsideUse : LobUse
    {
        protected override bool UseWithMap(int item)
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
