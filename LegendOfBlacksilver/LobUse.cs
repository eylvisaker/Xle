using AgateLib;
using System.Threading.Tasks;
using Xle.Services.Commands.Implementation;

namespace Xle.Blacksilver
{
    [Transient("LobUse")]
    public class LobUse : Use
    {
        public LobUse()
        {
            ShowItemMenu = true;
        }

        protected override async Task<bool> UseHealingItem(int itemID)
        {
            if (itemID == (int)LobItem.LifeElixir)
            {
                Player.Items[itemID] -= 1;
                await ApplyHealingEffect();
                return true;
            }

            return false;
        }
    }
}
