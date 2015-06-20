using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes.Extenders
{
    public class LeaveMap : EventExtender
    {
        public override bool StepOn(GameState state)
        {
            MapExtender.LeaveMap();

            return true;
        }
    }
}
