using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xle.XleEventTypes.Stores.Extenders
{
    public class StoreVault : StoreExtender
    {
        public override bool AllowInteractionWhenLoanOverdue { get { return true; } }

        public override Task<bool> Speak()
        {
            return Task.FromResult(false);
        }


        public override bool AllowRobWhenNotAngry
        {
            get
            {
                return true;
            }
        }
    }

}
