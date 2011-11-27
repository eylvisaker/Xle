using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using AgateLib;
using AgateLib.InputLib;
using AgateLib.Geometry;
using AgateLib.Serialization.Xle;
using AgateLib.DisplayLib;

namespace ERY.Xle.XleMapTypes
{
	public class Dungeon : Map3D
	{
		int mWidth;
		int mHeight;
		int mLevels = 1;

		int[] mData;
		int mCurrentLevel;

		public override IEnumerable<string> AvailableTilesets
		{
			get { yield return "DungeonTiles.png"; }
		}

		protected override void ReadData(XleSerializationInfo info)
		{
			mWidth = info.ReadInt32("Width");
			mHeight = info.ReadInt32("Height");
			mLevels = info.ReadInt32("Levels");
			mData = info.ReadInt32Array("Data");
		}
		protected override void WriteData(XleSerializationInfo info)
		{
			info.Write("Width", mWidth, true);
			info.Write("Height", mHeight, true);
			info.Write("Levels", mLevels, true);
			info.Write("Data", mData);
		}

		public override bool IsMultiLevelMap
		{
			get { return true; }
		}
		public override void InitializeMap(int width, int height)
		{
			mWidth = width;
			mHeight = height;

			mData = new int[mLevels * mHeight * mWidth];
		}
		public override void SetLevels(int count)
		{
			int[] newData = new int[count * Height * Width];
			int levelSize = Height * Width;

			for (int i = 0; i < Levels; i++)
			{
				for (int x = 0; x < Width; x++)
				{
					for (int y = 0; y < Height; y++)
					{
						int newIndex = i * levelSize + y * Width + x;
						int oldIndex = i * levelSize + y * Width + x;

						if (newIndex >= newData.Length ||
							oldIndex >= mData.Length)
						{
							goto done;
						}
						newData[newIndex] = mData[oldIndex];
					}
				}
			}

		done:
			mData = newData;
			mLevels = count;
		}
		public override int Height
		{
			get { return mHeight; }
		}
		public override int Width
		{
			get { return mWidth; }
		}
		public override int Levels
		{
			get
			{
				return mLevels;
			}
		}

		public int CurrentLevel
		{
			get { return mCurrentLevel; }
			set { mCurrentLevel = value; }
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
					return mData[mCurrentLevel * Height * Width + yy * Width + xx];
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
					mData[mCurrentLevel * Height * Width + yy * Width + xx] = value;
				}
			}
		}
		public override bool PlayerClimb(Player player)
		{
			switch (XleCore.Map[player.X, player.Y])
			{
				case 0x11:
					if (player.DungeonLevel == 0)
					{
						g.AddBottom("");
						g.AddBottom("You climb out of the dungeon.");

						// TODO: fix this
						player.ReturnToOutside();

						return true;
					}
					else
						player.DungeonLevel--;
					break;

				case 0x12:
					player.DungeonLevel++;
					break;

				default:
					return false;

			}

			DungeonLevelText(player);

			return true;
		}

		private void DungeonLevelText(Player player)
		{
			mCurrentLevel = player.DungeonLevel;
			
			string tempstring = "You are now at level " + (player.DungeonLevel + 1).ToString() + ".";

			g.AddBottom("");
			g.AddBottom(tempstring, XleColor.White);
		}

		protected override bool ShowDirections(Player player)
		{
			// check for compass.
			return player.Item(11) > 0;
		}
		public override void OnLoad(Player player)
		{
			base.OnLoad(player);

			player.beenInDungeon = true;
			CurrentLevel = player.DungeonLevel;
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

		#region --- Drawing ---

		protected override Surface Backdrop
		{
			get { return g.DungeonBlueBackdrop; }
		}
		protected override Surface Wall
		{
			get { return g.DungeonBlueWall; }
		}
		protected override Surface SidePassages
		{
			get { return g.DungeonBlueSidePassage; }
		}
		protected override Surface Door
		{
			get { return g.MuseumDoor; }
		}

		#endregion

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
			return false;
		}
		public override bool PlayerXamine(Player player)
		{
			Point faceDir = new Point();

			switch (player.FaceDirection)
			{
				case Direction.East: faceDir = new Point(1, 0); break;
				case Direction.West: faceDir = new Point(-1, 0); break;
				case Direction.North: faceDir = new Point(0, -1); break;
				case Direction.South: faceDir = new Point(0, 1); break;
				default: break;
			}

			g.AddBottom();

			bool revealHidden = false;

			for (int i = 0; i < 5; i++)
			{
				Point loc = new Point(player.X + faceDir.X * i, player.Y + faceDir.Y * i);

				if (this[loc.X, loc.Y] < 0x10)
					break;
				if (this[loc.X, loc.Y] >= 0x21 && this[loc.X, loc.Y] < 0x2a)
				{
					this[loc.X, loc.Y] -= 0x10;
					revealHidden = true;
				}
			}

			if (revealHidden)
			{
				g.AddBottom("Hidden objects detected!!!", Color.White);
			}


			XleCore.wait(500);
			return true;
		}
		protected override void OnPlayerEnterPosition(Player player, int x, int y)
		{
			int val = this[x, y];

			if (val >= 0x21 && val <= 0x2a)
			{
				OnPlayerTriggerTrap(player, x, y);
			}
			else if (val >= 0x11 && val <= 0x1a)
			{
				OnPlayerAvoidTrap(player, x, y);
			}
		}

		private void OnPlayerAvoidTrap(Player player, int x, int y)
		{
			// don't print a message for ceiling holes
			if (this[x, y] == 0x21) return;

			//string name = TrapName(this[x, y]);

			//g.AddBottom();
			//g.AddBottom("You avoid the " + name + ".");
			//XleCore.wait(150);
		}
		private void OnPlayerTriggerTrap(Player player, int x, int y)
		{
			// don't trigger ceiling holes
			if (this[x, y] == 0x21) return;

			this[x, y] -= 0x10;
			int damage = 31;
			g.AddBottom();

			if (this[x, y] == 0x12)
			{
				g.AddBottom("You fall through a hidden hole.", Color.White);
			}
			else
			{
				g.AddBottom("You're ambushed by a " + TrapName(this[x, y]) + ".", Color.White);
				XleCore.wait(100);
			}

			g.AddBottom("   H.P. - " + damage.ToString());
			player.HP -= damage;

			XleCore.wait(400);

			if (this[x, y] == 0x12)
			{
				player.DungeonLevel++;
				DungeonLevelText(player);
			}
		}

		string TrapName(int val)
		{
			switch (val)
			{
				case 0x11: return "ceiling hole";
				case 0x12: return "floor hole";
				case 0x13: return "poison gas vent";
				case 0x14: return "slime splotch";
				case 0x15: return "trip wire";
				default: throw new ArgumentException();
			}
		}
	}
}
