using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Citadel.EventExtenders
{
	class CitadelDoor : DoorExtender
	{
		public override void ItemUnlocksDoor(GameState state, int item, ref bool itemUnlocksDoor)
		{
			if (item == (int)LobItem.QuartzKey)
			{
				itemUnlocksDoor = true;
			}
			else
			{
				base.ItemUnlocksDoor(state, item, ref itemUnlocksDoor);
			}
		}
	}
}
