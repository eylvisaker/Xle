using System.Linq;

using ERY.Xle.Services.Implementation;
using ERY.Xle.XleEventTypes.Extenders;

namespace ERY.Xle.LotA.MapExtenders.Castle.Events
{
	class MagicIce : EventExtender
	{
		public override bool Use(GameState state, int item)
		{
			if (item != (int)LotaItem.MagicIce)
				return false;

			var iceGroup = state.Map.TileSet.TileGroups.FirstOrDefault(x => x.GroupType == Maps.GroupType.Special1);
			var waterGroup = state.Map.TileSet.TileGroups.FirstOrDefault(x => x.GroupType == Maps.GroupType.Water);

			if (iceGroup == null || iceGroup.Tiles.Count == 0 ||
				waterGroup == null || waterGroup.Tiles.Count == 0)
			{
				return false;
			}

			XleCore.Wait(250);

			for (int j = TheEvent.Rectangle.Top; j < TheEvent.Rectangle.Bottom; j++)
			{
				for (int i = TheEvent.Rectangle.Left; i < TheEvent.Rectangle.Right; i++)
				{
					int tile = state.Map[i, j];

					if (waterGroup.Tiles.Contains(tile))
					{
						state.Map[i, j] = iceGroup.Tiles[XleCore.random.Next(iceGroup.Tiles.Count)];
					}
				}
			}

			return true;
		}
	}
}
