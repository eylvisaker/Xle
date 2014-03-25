using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using AgateLib;
using AgateLib.InputLib;
using AgateLib.Geometry;
using AgateLib.Serialization.Xle;
using ERY.Xle.Maps.Extenders;


namespace ERY.Xle.Maps.XleMapTypes
{
	public class Town : Map2D
	{
		public new TownExtender Extender { get; protected set; }

		public List<int> Mail { get;set;}

		protected override MapExtender CreateExtenderImpl()
		{
			if (XleCore.Factory == null)
			{
				Extender = new TownExtender();
			}
			else
			{
				Extender = XleCore.Factory.CreateMapExtender(this);
			}

			return Extender;
		}

		#region --- Construction and Serialization ---

		public Town()
		{
			HasRoofs = true;
			HasGuards = true;
		}

		protected override void WriteData(XleSerializationInfo info)
		{
			base.WriteData(info);

			info.Write("Mail", Mail.ToArray());
		}
		protected override void ReadData(XleSerializationInfo info)
		{
			base.ReadData(info);

			Mail = info.ReadInt32Array("Mail").ToList();
		}

		#endregion

		public override IEnumerable<string> AvailableTileImages
		{
			get
			{
				yield return "towntiles.png";
			}
		}

		public int RoofTile(int xx, int yy)
		{
			foreach (var r in Roofs)
			{
				Rectangle boundingRect = r.Rectangle;

				if (boundingRect.Contains(new Point(xx, yy)))
				{
					var result = r[xx - r.X, yy - r.Y];

					if (result == 0 || result == 127)
						continue;

					if (r.Open)
						return 0;
				}
			}

			for (int i = 0; i < Roofs.Count; i++)
			{
				Roof r = Roofs[i];
				Rectangle boundingRect = r.Rectangle;

				if (boundingRect.Contains(new Point(xx, yy)))
				{
					var result = r[xx - r.X, yy - r.Y];

					if (result == 0 || result == 127)
						continue;

					return r[xx - r.X, yy - r.Y];
				}
			}

			return 0;
		}

		protected override void DrawImpl(int x, int y, Direction facingDirection, Rectangle inRect)
		{
			base.DrawImpl(x, y, facingDirection, inRect);

			DrawGuards(new Point(x, y), inRect);
		}

		protected void DrawGuards(Point centerPoint, Rectangle inRect)
		{
			Point topLeftMapPt = new Point(centerPoint.X - 11, centerPoint.Y - 7);

			int px = inRect.Left;
			int py = inRect.Top;

			for (int i = 0; i < Guards.Count; i++)
			{
				Guard guard = Guards[i];

				if (ClosedRoofAt(guard.X, guard.Y) == null)
				{
					var facing = guard.Facing;

					int rx = px + (guard.X - topLeftMapPt.X) * 16;
					int ry = py + (guard.Y - topLeftMapPt.Y) * 16;

					if (rx >= inRect.Left && ry >= inRect.Top && rx <= inRect.Right - 32 && ry <= inRect.Bottom - 32)
					{
						XleCore.Renderer.DrawCharacterSprite(rx, ry, facing, true, Guards.AnimFrame, false, guard.Color);
					}
				}
			}
		}
	}
}
