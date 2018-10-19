using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xle.XleEventTypes.Stores.Extenders
{
    public class StoreMinigame : StoreExtender
    { }

    public class StoreWeaponTraining : StoreExtender
    {
    }
    public class StoreArmorTraining : StoreExtender
    {
    }


    public class StoreBlackjack : StoreExtender
    {
        public override bool AllowInteractionWhenLoanOverdue { get { return true; } }

    }


    public class StoreFlipFlop : StoreExtender
    {
        public override bool AllowInteractionWhenLoanOverdue { get { return true; } }

    }
}
