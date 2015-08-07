using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Services;
using ERY.Xle.Services.Commands.Implementation;

namespace ERY.Xle.LoB
{
    [ServiceName("LobUse")]
    public class LobUse : Use
    {
        public LobUse()
        {
            ShowItemMenu = true;
        }

        protected override bool UseHealingItem(int itemID)
        {
            if (itemID == (int)LobItem.LifeElixir)
            {
                Player.Items[itemID] -= 1;
                ApplyHealingEffect();
                return true;
            }

            return false;
        }
    }
}
