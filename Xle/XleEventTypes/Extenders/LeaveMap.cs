using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xle.XleEventTypes.Extenders
{
    public class LeaveMap : EventExtender
    {
        public override async Task<bool> StepOn()
        {
            MapExtender.LeaveMap();

            return true;
        }
    }
}
