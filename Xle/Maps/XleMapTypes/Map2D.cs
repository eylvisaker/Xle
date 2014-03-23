using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.Serialization.Xle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Maps.XleMapTypes
{
	public abstract class Map2D : XleMap
	{
		int mWidth;
		int mHeight;
		int[] mData;
		int mOutsideTile = 0;

		protected override void WriteData(AgateLib.Serialization.Xle.XleSerializationInfo info)
		{
			info.Write("Width", mWidth);
			info.Write("Height", mHeight);
			info.Write("OutsideTile", mOutsideTile);
			info.Write("MapData", mData, NumericEncoding.Csv);

			base.WriteData(info);
		}

		protected override void ReadData(AgateLib.Serialization.Xle.XleSerializationInfo info)
		{
			mWidth = info.ReadInt32("Width");
			mHeight = info.ReadInt32("Height");
			mOutsideTile = info.ReadInt32("OutsideTile");
			mData = info.ReadInt32Array("MapData");

			base.ReadData(info);
		}


		public override void InitializeMap(int width, int height)
		{
			mWidth = width;
			mHeight = height;

			mData = new int[mWidth * mHeight];
		}

		public override int Height
		{
			get { return mHeight; }
		}
		public override int Width
		{
			get { return mWidth; }
		}
		public int OutsideTile
		{
			get { return mOutsideTile; }
			set { mOutsideTile = value; }
		}

		public override int this[int xx, int yy]
		{
			get
			{
				if (yy < 0 || yy >= Height || xx < 0 || xx >= Width)
				{
					var outsideTile = mBaseExtender.GetOutsideTile(centerPoint, xx, yy);

					if (outsideTile == -1)
						return mOutsideTile;
					else
						return outsideTile;
				}
				else
				{
					return mData[xx + yy * mWidth];
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
					mData[xx + yy * mWidth] = value;
				}
			}
		}

		public override bool CanPlayerStepIntoImpl(Player player, int xx, int yy)
		{
			int test = 0;

			if (GuardInSpot(xx, yy))
				return false;

			for (int j = 0; j < 2; j++)
			{
				for (int i = 0; i < 2; i++)
				{
					test = this[xx + i, yy + j];

					if (IsTileBlocked(test))
						return false;
				}
			}

			return true;
		}

		protected virtual bool GuardInSpot(int xx, int yy)
		{
			return false;
		}

		protected virtual bool IsTileBlocked(int tile)
		{
			return TileSet[tile] == TileInfo.Blocked;
		}

		protected override void DrawImpl(int x, int y, Direction facingDirection, Rectangle inRect)
		{
			Draw2D(x, y, facingDirection, inRect);
		}


		protected abstract void PlayerFight(Player player, Direction fightDir);
	}
}
