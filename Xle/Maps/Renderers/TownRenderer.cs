using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ERY.Xle.Services.Implementation;

namespace ERY.Xle.Maps.Renderers
{
	public class TownRenderer : Map2DRenderer
	{
		protected override void Animate(AgateLib.Geometry.Rectangle tileRect)
		{
			base.Animate(tileRect);

			TheMap.Guards.AnimateGuards();
		}

		public override void Draw(Point playerPos, Direction faceDirection, Rectangle inRect)
		{
			base.Draw(playerPos, faceDirection, inRect);

			DrawGuards(playerPos, inRect);
		}

		protected override int TileToDraw(int x, int y)
		{
			int tile = base.TileToDraw(x, y);

			var roof = TheMap.ClosedRoofAt(x, y);

			if (roof == null)
				return tile;

			var roofTile = roof.TileAtMapCoords(x, y);

			if (roofTile == 0)
				return tile;

			return roofTile;
		}

		protected void DrawGuards(Point playerPos, Rectangle inRect)
		{
			Point topLeftMapPt = new Point(playerPos.X - 11, playerPos.Y - 7);

			int px = inRect.Left;
			int py = inRect.Top;

			for (int i = 0; i < TheMap.Guards.Count; i++)
			{
				Guard guard = TheMap.Guards[i];

				if (TheMap.ClosedRoofAt(guard.X, guard.Y) == null)
				{
					var facing = guard.Facing;

					int rx = px + (guard.X - topLeftMapPt.X) * 16;
					int ry = py + (guard.Y - topLeftMapPt.Y) * 16;

					if (rx >= inRect.Left && ry >= inRect.Top && rx <= inRect.Right - 32 && ry <= inRect.Bottom - 32)
					{
						XleCore.Renderer.DrawCharacterSprite(rx, ry, facing, true, TheMap.Guards.AnimFrame, false, guard.Color);
					}
				}
			}
		}
	}
}
