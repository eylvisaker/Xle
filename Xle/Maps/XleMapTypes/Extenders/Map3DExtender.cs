using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Maps.XleMapTypes.Extenders
{
	public class Map3DExtender : MapExtender
	{
		protected void _MoveDungeon(Player player, Direction dir, bool haveCompass, out string command, out Point stepDirection)
		{
			Direction newDirection;
			command = "";
			stepDirection = Point.Empty;

			switch (dir)
			{
				case Direction.East:
					command = "Turn Right";

					newDirection = player.FaceDirection - 1;

					if (newDirection < Direction.East)
						newDirection = Direction.South;


					player.FaceDirection = (Direction)newDirection;

					if (haveCompass)
						command = "Turn " + player.FaceDirection.ToString();

					break;

				case Direction.North:
					command = "Move Forward";

					stepDirection = StepDirection(player.FaceDirection);

					if (haveCompass)
						command = "Walk " + player.FaceDirection.ToString();

					player.TimeQuality += TheMap.StepQuality;

					break;

				case Direction.West:
					command = "Turn Left";

					newDirection = player.FaceDirection + 1;


					if (newDirection > Direction.South)
						newDirection = Direction.East;

					player.FaceDirection = (Direction)newDirection;

					if (haveCompass)
						command = "Turn " + player.FaceDirection.ToString();

					break;

				case Direction.South:
					command = "Move Backward";

					if (player.FaceDirection == Direction.East)
						stepDirection = new Point(-1, 0);
					if (player.FaceDirection == Direction.North)
						stepDirection = new Point(0, 1);
					if (player.FaceDirection == Direction.West)
						stepDirection = new Point(1, 0);
					if (player.FaceDirection == Direction.South)
						stepDirection = new Point(0, -1);

					if (haveCompass)
					{
						// we're walking backwards here, so make the text work right!
						command = "Walk ";
						switch (player.FaceDirection)
						{
							case Direction.East: command += "West"; break;
							case Direction.West: command += "East"; break;
							case Direction.North: command += "South"; break;
							case Direction.South: command += "North"; break;
						}
					}

					player.TimeQuality += TheMap.StepQuality;


					break;
			}
		}

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
