using AgateLib;
using Xle.Services.Commands.Implementation;

namespace Xle.Ancients
{
    [Transient("LotaUse")]
    public class LotaUse : Use
    {
        public LotaUse()
        {
            ShowItemMenu = false;
        }

        protected override bool UseHealingItem(int itemID)
        {
            if (itemID == (int)LotaItem.HealingHerb)
            {
                Player.Items[itemID] -= 1;
                ApplyHealingEffect();
                return true;
            }

            return false;
        }
    }
}
