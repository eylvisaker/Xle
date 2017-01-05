using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using AgateLib;
using AgateLib.InputLib;
using AgateLib.Geometry;
using ERY.Xle.Serialization;

using ERY.Xle.XleEventTypes.Stores;

namespace ERY.Xle.Maps.XleMapTypes
{
    public class Outside : XleMap
    {
        int[] mData;
        int mHeight;
        int mWidth;

        #region --- Construction and Serialization ---

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

        public override bool StoreReturnLocation
        {
            get { return true; }
        }

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
                    mData[xx + mWidth * yy] = value;
                }
            }
        }

    }
}