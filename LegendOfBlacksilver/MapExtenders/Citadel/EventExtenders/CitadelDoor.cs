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
		public override bool ItemUnlocksDoor(GameState state, int item)
		{
			if (item == (int)LobItem.QuartzKey)
			{
				return true;
			}
			else
			{
				return base.ItemUnlocksDoor(state, item);
			}
		}
	}
}
