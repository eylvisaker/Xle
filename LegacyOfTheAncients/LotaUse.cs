using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Services;
using ERY.Xle.Services.Commands.Implementation;

namespace ERY.Xle.LotA
{
	[ServiceName("LotaUse")]
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
