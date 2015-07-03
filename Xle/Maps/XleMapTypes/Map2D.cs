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

        protected override void WriteData(AgateLib.Serialization.Xle.XleSerializationInfo info)
        {
            info.Write("Width", mWidth);
            info.Write("Height", mHeight);
            info.Write("MapData", mData, NumericEncoding.Csv);

            base.WriteData(info);
        }
        protected override void ReadData(AgateLib.Serialization.Xle.XleSerializationInfo info)
        {
            mWidth = info.ReadInt32("Width");
            mHeight = info.ReadInt32("Height");
            mData = info.ReadArray<int>("MapData");

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

        public override int this[int xx, int yy]
        {
            get
            {
                if (yy < 0 || yy >= Height || xx < 0 || xx >= Width)
                {
                    return OutsideTile;
                }

                return mData[xx + yy * mWidth];
            }
            set
            {
                if (yy < 0 || yy >= Height ||
                    xx < 0 || xx >= Width)
                {
                    return;
                }

                mData[xx + yy * mWidth] = value;
            }
        }
    }
}
