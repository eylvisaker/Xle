using AgateLib;
using System.Threading.Tasks;
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

        protected override  async Task<bool> UseHealingItem(int itemID)
        {
            if (itemID == (int)LotaItem.HealingHerb)
            {
                Player.Items[itemID] -= 1;
                await ApplyHealingEffect();
                return true;
            }

            return false;
        }
    }
}
