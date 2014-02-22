using ERY.Xle.XleEventTypes;
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

	class FeatherDoor : CastleDoor
	{
		public override void RemoveDoor(GameState state, ref bool handled)
		{
			var rect = TheEvent.Rectangle;
			var doorEvent = (Door)TheEvent;

			for (int j = rect.Y; j < rect.Bottom; j++)
			{
				for (int i = rect.X; i < rect.Right; i++)
				{
					state.Map[i, j] = doorEvent.ReplacementTile;
				}
			}

			for (int j = rect.Y + 1; j < rect.Bottom; j += 2)
			{
				int i = rect.X + 1;

				state.Map[i, j] = doorEvent.ReplacementTile;
			}
		}
	}
}
