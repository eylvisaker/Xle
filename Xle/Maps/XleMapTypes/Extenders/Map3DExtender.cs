using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Maps.XleMapTypes.Extenders
{
	public class Map3DExtender : MapExtender
	{
		public override bool CanPlayerStepIntoImpl(Player player, int xx, int yy)
		{
			if (IsMapSpaceBlocked(xx, yy))
				return false;

			return true;
		}

		public static Point StepDirection(Direction dir)
		{
			switch (dir)
			{
				case Direction.East: return new Point(1, 0);
				case Direction.North: return new Point(0, -1);
				case Direction.West: return new Point(-1, 0);
				case Direction.South: return new Point(0, 1);

				default: throw new ArgumentException("Invalid direction!");
			}
		}
		public static Point LeftDirection(Direction dir)
		{
			switch (dir)
			{
				case Direction.East: return StepDirection(Direction.North);
				case Direction.North: return StepDirection(Direction.West);
				case Direction.West: return StepDirection(Direction.South);
				case Direction.South: return StepDirection(Direction.East);

				default: throw new ArgumentException("Invalid direction!");
			}
		}
		public static Point RightDirection(Direction dir)
		{
			switch (dir)
			{
				case Direction.East: return StepDirection(Direction.South);
				case Direction.North: return StepDirection(Direction.East);
				case Direction.West: return StepDirection(Direction.North);
				case Direction.South: return StepDirection(Direction.West);

				default: throw new ArgumentException("Invalid direction!");
			}
		}

		protected bool IsMapSpaceBlocked(int xx, int yy)
		{
			if (TheMap[xx, yy] >= 0x40)
				return true;
			else if ((TheMap[xx, yy] & 0xf0) == 0x00)
				return true;

			return false;
		}

	}
}
