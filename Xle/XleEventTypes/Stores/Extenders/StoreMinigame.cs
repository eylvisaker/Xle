using AgateLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xle.XleEventTypes.Stores.Extenders
{
    public class StoreMinigame : StoreExtender
    { }

    [Transient("StoreWeaponTraining")]
    public class StoreWeaponTraining : StoreExtender
    {
    }

    [Transient("StoreArmorTraining")]

    public class StoreArmorTraining : StoreExtender
    {
    }

    [Transient("StoreBlackjack")]
    public class StoreBlackjack : StoreExtender
    {
        public override bool AllowInteractionWhenLoanOverdue { get { return true; } }

    }

    [Transient("StoreFlipFlop")]
    public class StoreFlipFlop : StoreExtender
    {
        public override bool AllowInteractionWhenLoanOverdue { get { return true; } }

    }
}
