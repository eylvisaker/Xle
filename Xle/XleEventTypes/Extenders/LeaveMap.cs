using AgateLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xle.XleEventTypes.Extenders
{
    [Transient("LeaveMap")]
    public class LeaveMap : EventExtender
    {
        public override async Task<bool> StepOn()
        {
            await MapExtender.LeaveMap();

            return true;
        }
    }
}
