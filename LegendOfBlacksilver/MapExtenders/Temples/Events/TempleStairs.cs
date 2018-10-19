using Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xle.LoB.MapExtenders.Temples
{
    public class TempleStairs : ChangeMap
    {
        public TempleStairs()
        {
            base.Enabled = false;
        }

        protected override bool OnStepOnImpl(ref bool cancel)
        {
            return false;
        }
    }
}
