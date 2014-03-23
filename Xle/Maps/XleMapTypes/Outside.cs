using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using AgateLib;
using AgateLib.InputLib;
using AgateLib.Geometry;
using AgateLib.Serialization.Xle;
using ERY.Xle.XleEventTypes.Stores;
using ERY.Xle.Maps.XleMapTypes.Extenders;


namespace ERY.Xle.Maps.XleMapTypes
{
	public class Outside : XleMap
	{
		int[] mData;
		int mHeight;
		int mWidth;

		int[] waves;
		Rectangle drawRect;

		List<Monster> currentMonst = new List<Monster>();

		int stepCount;
		public int displayMonst = -1;
		Direction monstDir;
		Point mDrawMonst;
		int monstCount, initMonstCount;
		bool isMonsterFriendly;
		int mWaterAnimLevel;

		#region --- Construction and Serialization ---

		public Outside()
		{ }

		protected override void WriteData(XleSerializationInfo info)
		{
			info.Write("Width", Width);
			info.Write("Height", Height);
			info.Write("MapData", mData, NumericEncoding.Csv);
		}
		protected override void ReadData(XleSerializationInfo info)
		{
			mWidth = info.ReadInt32("Width");
			mHeight = info.ReadInt32("Height");

			mData = info.ReadInt32Array("MapData");

		}

		#endregion


		public TerrainType TerrainAt(int xx, int yy)
		{
			int[,] t = new int[2, 2] { { 0, 0 }, { 0, 0 } };
			int[] tc = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };

			for (int j = 0; j < 2; j++)
			{
				for (int i = 0; i < 2; i++)
				{
					t[j, i] = this[xx + i, yy + j];
				}
			}

			for (int j = 0; j < 2; j++)
			{
				for (int i = 0; i < 2; i++)
				{
					tc[t[j, i] / 32]++;

					if (t[j, i] % 32 <= 1)
						tc[t[j, i] / 32] += 1;
				}
			}

			if (tc[(int)TerrainType.Mountain] > 4)
			{
				return TerrainType.Mountain;
			}

			if (tc[(int)TerrainType.Mountain] > 0)
			{
				return TerrainType.Foothills;
			}

			if (tc[(int)TerrainType.Desert] >= 1)
			{
				return TerrainType.Desert;
			}

			if (tc[(int)TerrainType.Swamp] > 1)
			{
				return TerrainType.Swamp;
			}

			for (int i = 0; i < 8; i++)
			{
				if (tc[i] > 3)
				{
					return (TerrainType)i;
				}
				else if (tc[i] == 2 && i != 1)
				{
					return TerrainType.Mixed;
				}
			}

			return (TerrainType)2;
		}

		public override void InitializeMap(int width, int height)
		{
			mWidth = width;
			mHeight = height;
			mData = new int[mWidth * mHeight];
		}
		public override IEnumerable<string> AvailableTileImages
		{
			get
			{
				yield return "tiles.png";
			}
		}

		public override int Height
		{
			get { return mHeight; }
		}
		public override int Width
		{
			get { return mWidth; }
		}
		public override int this[int xx, int yy]
		{
			get
			{
				int result;

				if (yy < 0 || yy >= Height || xx < 0 || xx >= Width)
					result = 0;
				else
					result = mData[xx + mWidth * yy];

				if (result == 0 && waves != null)
				{
					int index = xx - drawRect.Left + (yy - drawRect.Top) * drawRect.Width;

					if (index >= 0 && index < waves.Length)
						return waves[index];
				}

				return result;
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
					// mData[yy, xx] = value;
					//mData[(yy + mapExtend) * (mapWidth + 2 * mapExtend) + (xx + mapExtend)] = (byte)val;

					mData[xx + mWidth * yy] = value;
				}

			}
		}
		
		/// <summary>
		/// Gets or sets whether or not the player is in stormy water
		/// </summary>
		/// <returns></returns>
		public int WaterAnimLevel
		{
			get { return mWaterAnimLevel; }
			set
			{
				System.Diagnostics.Debug.Assert(value >= 0);

				mWaterAnimLevel = value;
			}
		}

		int lastAnimate = 0;


		protected override void AnimateTiles(Rectangle rectangle)
		{
			int now = (int)Timing.TotalMilliseconds;

			if (rectangle != drawRect)
			{
				ClearWaves();

				drawRect = rectangle;
			}
			if (lastAnimate + 250 > now)
				return;

			if (waves == null || waves.Length != rectangle.Width * rectangle.Height)
			{
				waves = new int[rectangle.Width * rectangle.Height];
			}

			lastAnimate = now;

			for (int j = 0; j < rectangle.Height; j++)
			{
				for (int i = 0; i < rectangle.Width; i++)
				{
					int x = i + rectangle.Left;
					int y = j + rectangle.Top;
					int index = j * rectangle.Width + i;

					int tile = this[x, y];

					if (tile == 0)
					{
						if (XleCore.random.Next(0, 1000) < 20 * (WaterAnimLevel + 1))
						{
							waves[index] = XleCore.random.Next(1, 3);
						}
					}
					else if (tile == 1 || tile == 2)
					{
						if (XleCore.random.Next(0, 100) < 25)
						{
							waves[index] = 0;
						}
					}
				}
			}
		}

		public void ClearWaves()
		{
			if (waves != null)
				Array.Clear(waves, 0, waves.Length);

			int now = (int)Timing.TotalMilliseconds;

			// force an update.
			lastAnimate = now - 500;
		}

		protected override void DrawImpl(int x, int y, Direction facingDirection, Rectangle inRect)
		{
			Draw2D(x, y, facingDirection, inRect);

			if (displayMonst > -1)
			{
				Point pt = new Point(mDrawMonst.X - x, mDrawMonst.Y - y);
				pt.X *= 16;
				pt.Y *= 16;

				pt.X += XleCore.Renderer.CharRect.X;
				pt.Y += XleCore.Renderer.CharRect.Y;

				XleCore.Renderer.DrawMonster(pt.X, pt.Y, displayMonst);
			}
		}

		[Obsolete("This function is weird and should be replaced with something else.")]
		public string MonsterDirection(Player player)
		{
			string dirName;
			mDrawMonst.X = player.X - 1;
			mDrawMonst.Y = player.Y - 1;

			monstDir = (Direction)XleCore.random.Next((int)Direction.East, (int)Direction.South + 1);

			switch (monstDir)
			{
				case Direction.East: dirName = "East"; mDrawMonst.X += 2; break;
				case Direction.North: dirName = "North"; mDrawMonst.Y -= 2; break;
				case Direction.West: dirName = "West"; mDrawMonst.X -= 2; break;
				case Direction.South: dirName = "South"; mDrawMonst.Y += 2; break;
				default:
					throw new Exception("Invalid direction.");
			}
			return dirName;
		}

		protected override Extenders.MapExtender CreateExtenderImpl()
		{
			if (XleCore.Factory == null)
			{
				Extender = new OutsideExtender();
			}
			else
			{
				Extender = XleCore.Factory.CreateMapExtender(this);
			}

			return Extender;
		}

		public override int WaitTimeAfterStep
		{
			get
			{
				return XleCore.GameState.GameSpeed.OutsideStepTime;
			}
		}

		public new OutsideExtender Extender { get; private set; }
	}
}