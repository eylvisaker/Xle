using AgateLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xle.XleEventTypes.Stores.Extenders
{
    [Transient("StoreJail")]
    public class StoreJail : StoreExtender
    {
        public override bool AllowInteractionWhenLoanOverdue { get { return true; } }

    }
}
