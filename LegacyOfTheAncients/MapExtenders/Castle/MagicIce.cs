﻿using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.MapExtenders.Castle
{
	class MagicIce : NullEventExtender
	{
		public override void Use(GameState state, int item, ref bool handled)
		{
			if (item != (int)LotaItem.MagicIce)
				return;

			var iceGroup = state.Map.TileSet.TileGroups.FirstOrDefault(x => x.GroupType == Maps.GroupType.Special1);
			var waterGroup = state.Map.TileSet.TileGroups.FirstOrDefault(x => x.GroupType == Maps.GroupType.Water);

			if (iceGroup == null || iceGroup.Tiles.Count == 0 ||
				waterGroup == null || waterGroup.Tiles.Count == 0)
				return;

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

			handled = true;
		}
	}
}