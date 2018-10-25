using Xle.Ancients.MapExtenders.Castle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xle.Ancients.MapExtenders.Castle.Events;

namespace Xle.Ancients.MapExtenders.Fortress.SecondArea
{
    public class FinalMagicIce : MagicIce
    {
        private bool activatedCompendium;
        private IFortressFinalActivator fortressActivator;

        public FinalMagicIce(IFortressFinalActivator fortressActivator)
        {
            this.fortressActivator = fortressActivator;
        }

        FortressFinal fortressFinal { get { return (FortressFinal)GameState.MapExtender; } }

        public override async Task<bool> Use(int item)
        {
            await base.Use(item);

            if (item == (int)LotaItem.MagicIce && activatedCompendium == false)
            {
                activatedCompendium = true;
                fortressActivator.CompendiumAttacking = true;
            }
            return true;
        }
    }
}
