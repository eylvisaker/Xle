using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Castle.EventExtenders
{
	class CastleDoor : DoorExtender
	{
		public override void ItemUnlocksDoor(GameState state, int item, ref bool itemUnlocksDoor)
		{
			if (item == 7)
				itemUnlocksDoor = true;
		}
	}
}
