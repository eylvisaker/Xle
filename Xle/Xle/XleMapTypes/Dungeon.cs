using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using AgateLib;
using AgateLib.InputLib;
using AgateLib.Geometry;
using AgateLib.Serialization.Xle;


namespace ERY.Xle.XleMapTypes
{
	public class Dungeon : Map3D
	{
		/// <summary>
		/// Dungeon data.  Index order is mData[level, y, x].
		/// </summary>
		int[, ,] mData;
		int mCurrentLevel;

		public override bool IsMultiLevelMap
		{
			get { return true; }
		}
		public override void InitializeMap(int width, int height)
		{
			mData = new int[1, height, width];
		}
		public override void SetLevels(int count)
		{
			int[, ,] newData = new int[count, Height, Width];

			for (int i = 0; i < Levels; i++)
			{
				for (int x = 0; x < Width; x++)
				{
					for (int y = 0; y < Height; y++)
					{
						newData[i, y, x] = mData[i, y, x];
					}
				}
			}
		}
		public override int Height
		{
			get { return mData.GetUpperBound(1) + 1; }
		}
		public override int Width
		{
			get { return mData.GetUpperBound(2) + 1; }
		}
		public override int Levels
		{
			get
			{
				return mData.GetUpperBound(0) + 1;
			}
		}

		public override Color DefaultColor
		{
			get
			{
				return XleColor.Cyan;
			}
		}
		public override int this[int xx, int yy]
		{
			get
			{
				if (yy < 0 || yy >= Height || xx < 0 || xx >= Width)
				{
					return 0;
				}
				else
				{
					return mData[mCurrentLevel, yy, xx];
				}
			}
			set
			{
				if (yy < 0 || yy >= Height ||
					xx < 0 || xx >= Width)
				{
					return;
				}
				else
				{
					mData[mCurrentLevel, yy, xx] = value;
					//mData[(yy + mapExtend) * (mapWidth + 2 * mapExtend) + (xx + mapExtend)] = (byte)val;
				}

			}
		}
		public override bool PlayerClimb(Player player)
		{
			switch (XleCore.Map[player.X, player.Y])
			{
				case 0x0D:
					player.DungeonLevel--;
					break;

				case 0x0A:
					player.DungeonLevel++;
					break;

				default:
					return false;

			}

			mCurrentLevel = player.DungeonLevel - 1;

			if (player.DungeonLevel == 0)
			{
				g.AddBottom("");
				g.AddBottom("You climb out of the dungeon.");

				// TODO: fix this
				player.ReturnToOutside();
			}
			else
			{
				string tempstring = "You are now at level " + player.DungeonLevel.ToString() + ".";

				g.AddBottom("");
				g.AddBottom(tempstring, XleColor.White);

			}

			return true;
		}

		public override void PlayerCursorMovement(Player player, Direction dir)
		{
			string command;
			Point stepDirection;

			_MoveDungeon(player, dir, player.Item(11) > 0,  out command, out stepDirection);

			Commands.UpdateCommand(command);

			if (stepDirection.IsEmpty == false)
			{
				player.Move(stepDirection.X, stepDirection.Y);
			}

			Commands.UpdateCommand(command);
		}


		public override string[] MapMenu()
		{
			List<string> retval = new List<string>();


			retval.Add("Armor");
			retval.Add("Climb");
			retval.Add("End");
			retval.Add("Fight");
			retval.Add("Gamespeed");
			retval.Add("Hold");
			retval.Add("Inventory");
			retval.Add("Magic");
			retval.Add("Open");
			retval.Add("Pass");
			retval.Add("Use");
			retval.Add("Weapon");
			retval.Add("Xamine");

			return retval.ToArray();
		}

		protected override AgateLib.DisplayLib.Surface Backdrop
		{
			get { throw new NotImplementedException(); }
		}
		protected override AgateLib.DisplayLib.Surface Wall
		{
			get { throw new NotImplementedException(); }
		}
		protected override AgateLib.DisplayLib.Surface SidePassages
		{
			get { throw new NotImplementedException(); }
		}

		protected override bool CheckMovementImpl(Player player, int dx, int dy)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override void GetBoxColors(out Color boxColor, out Color innerColor, out Color fontColor, out int vertLine)
		{
			fontColor = XleColor.White;

			boxColor = XleColor.Gray;
			innerColor = XleColor.LightGreen;
			fontColor = XleColor.Cyan;
			vertLine = 15 * 16;

		}

		public override bool PlayerFight(Player player)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override bool CanPlayerStepInto(Player player, int xx, int yy)
		{
			int t = 0;

			if (this[xx, yy] >= 0x80)
			{
				t = 3;
			}
			else
				t = 0;



			if (t > 0)
			{
				return false;
			}
			else
				return true;
		}

	}
}
