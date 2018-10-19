using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xle.XleEventTypes.Extenders
{
    public class LeaveMap : EventExtender
    {
        public override bool StepOn()
        {
            MapExtender.LeaveMap();

            return true;
        }
    }
}
