using AgateLib.Geometry;
using ERY.Xle.Maps;
using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Castle.EventExtenders
{
	class SingingCrystal : EventExtender
	{
		public override void Use(GameState state, int item, ref bool handled)
		{
			if (item != (int)LobItem.SingingCrystal)
				return;

			handled = true;

			Rectangle area = new Rectangle(state.Player.X - 2, state.Player.Y - 3, 6, 8);

			RemoveRockSlide(state.Map, area);

			if (area.Right >= TheEvent.Rectangle.Right - 3)
			{
				SoundMan.PlaySound(LotaSound.VeryBad);
				state.Player.Items[LobItem.SingingCrystal] = 0;

				XleCore.TextArea.PrintLine("Your singing crystal melts.");

				XleCore.TextArea.FlashLinesWhile(() => SoundMan.IsPlaying(LotaSound.VeryBad), XleColor.Yellow, XleColor.Red, 250);

				Lob.Story.ClearedRockSlide = true;
			}
		}

		public static void RemoveRockSlide(XleMap map, Rectangle area)
		{
			var group = map.TileSet.TileGroups.FirstOrDefault(x => x.GroupType == Maps.GroupType.Special1);

			var replacementTile = 6;

			for (int j = area.Top; j < area.Bottom; j++)
			{
				for (int i = area.Left; i < area.Right; i++)
				{
					var tile = map[i, j];
					if (group.Tiles.Contains(tile))
					{
						map[i, j] = replacementTile;
					}
				}
			}
		}
	}
}
