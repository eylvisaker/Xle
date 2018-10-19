using Xle.Serialization;
using System.Collections.Generic;

namespace Xle.Maps.XleMapTypes
{
    public class Museum : Map3D
    {
        private int[] mData;
        private int mHeight;
        private int mWidth;

        protected override void ReadData(XleSerializationInfo info)
        {
            mWidth = info.ReadInt32("Width");
            mHeight = info.ReadInt32("Height");
            mData = info.ReadArray<int>("Data");
        }
        protected override void WriteData(XleSerializationInfo info)
        {
            info.Write("Width", mWidth, true);
            info.Write("Height", mHeight, true);
            info.Write("Data", mData, NumericEncoding.Csv);
        }

        public override IEnumerable<string> AvailableTileImages
        {
            get { yield return "DungeonTiles.png"; }
        }
        public override void InitializeMap(int width, int height)
        {
            mData = new int[height * width];
            mHeight = height;
            mWidth = width;
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
                    return 0;
                }
                else
                {
                    return mData[yy * mWidth + xx];
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
                    mData[yy * mWidth + xx] = value;
                }

            }
        }

        public override double StepQuality
        {
            get
            {
                return 0.5;
            }
        }

        private bool IsExhibit(int value)
        {
            if ((value & 0xf0) == 0x50)
                return true;
            else
                return false;
        }
    }
}