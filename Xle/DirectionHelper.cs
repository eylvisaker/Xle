using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle
{
	public static class DirectionHelper
	{
		public static Direction ToDirection(this Point point)
		{
			if (point.X < 0 && point.Y == 0) return Direction.West;
			if (point.X > 0 && point.Y == 0) return Direction.East;
			if (point.X == 0 && point.Y < 0) return Direction.North;
			if (point.X == 0 && point.Y > 0) return Direction.South;

			throw new ArgumentException();
		}

		public static Point StepDirection(this Direction dir)
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
		public static Point LeftDirection(this Direction dir)
		{
			switch (dir)
			{
				case Direction.East: return Direction.North.StepDirection();
				case Direction.North: return Direction.West.StepDirection();
				case Direction.West: return Direction.South.StepDirection();
				case Direction.South: return Direction.East.StepDirection();

				default: throw new ArgumentException("Invalid direction!");
			}
		}
		public static Point RightDirection(this Direction dir)
		{
			switch (dir)
			{
				case Direction.East: return Direction.South.StepDirection();
				case Direction.North: return Direction.East.StepDirection();
				case Direction.West: return Direction.North.StepDirection();
				case Direction.South: return Direction.West.StepDirection();

				default: throw new ArgumentException("Invalid direction!");
			}
		}

	}
}
