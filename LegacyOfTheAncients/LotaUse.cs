using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xle.Services;
using Xle.Services.Commands.Implementation;

namespace Xle.Ancients
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
