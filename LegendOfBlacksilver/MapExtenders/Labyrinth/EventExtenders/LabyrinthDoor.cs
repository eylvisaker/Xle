using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Labyrinth.EventExtenders
{
	class LabyrinthDoor : DoorExtender
	{
		public override void ItemUnlocksDoor(GameState state, int item, ref bool itemUnlocksDoor)
		{
			if (item == (int)LobItem.SkeletonKey)
			{
				itemUnlocksDoor = true;
				return;
			}

			base.ItemUnlocksDoor(state, item, ref itemUnlocksDoor);
		}
	}
}
