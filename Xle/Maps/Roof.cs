using AgateLib.Mathematics.Geometry;
using ERY.Xle.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Maps
{
    public class Roof : IXleSerializable
    {
        Rectangle mRect;
        int[] mData;

        private bool mOpen;

        #region --- Construction and Serialization ---

        public Roof()
        { }

        void IXleSerializable.WriteData(XleSerializationInfo info)
        {
            info.Write("X", X);
            info.Write("Y", Y);
            info.Write("Width", Width);
            info.Write("Height", Height);
            info.Write("RoofData", mData, NumericEncoding.Csv);
        }

        void IXleSerializable.ReadData(XleSerializationInfo info)
        {
            mRect.X = info.ReadInt32("X");
            mRect.Y = info.ReadInt32("Y");
            mRect.Width = info.ReadInt32("Width");
            mRect.Height = info.ReadInt32("Height");
            mData = info.ReadInt32Array("RoofData");
        }

        #endregion

        public Point Location
        {
            get { return mRect.Location; }
            set { mRect.Location = value; }
        }
        public int X
        {
            get { return mRect.X; }
            set { mRect.X = value; }
        }
        public int Y
        {
            get { return mRect.Y; }
            set { mRect.Y = value; }
        }

        public bool Open
        {
            get { return mOpen; }
            set { mOpen = value; }
        }

        public void SetSize(int width, int height)
        {
            int[] newData = new int[height * width];

            // copy old data to new data
            if (mData != null)
            {
                for (int i = 0; i < Math.Min(Width, width); i++)
                {
                    for (int j = 0; j < Math.Min(Height, height); j++)
                    {
                        newData[i + j * width] = mData[i + j * Width];
                    }
                }
            }

            mRect.Width = width;
            mRect.Height = height;

            mData = newData;
        }
        public int Width
        {
            get { return mRect.Width; }
        }
        public int Height
        {
            get { return mRect.Height; }
        }

        public int this[int x, int y]
        {
            get { return mData[x + y * Width]; }
            set { mData[x + y * Width] = value; }
        }

        public int TileAtMapCoords(int mapx, int mapy)
        {
            return this[mapx - Location.X, mapy - Location.Y];
        }

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle(Location, new Size(Width, Height));
            }
        }
        
        public bool CharIn(int ptx, int pty, bool ignoreTransparency = false)
        {
            if (PointInRoof(ptx, pty, ignoreTransparency))
            {
                return true;
            }
            else if (PointInRoof(ptx + 1, pty, ignoreTransparency))
            {
                return true;
            }
            else if (PointInRoof(ptx, pty + 1, ignoreTransparency))
            {
                return true;
            }
            else if (PointInRoof(ptx + 1, pty + 1, ignoreTransparency))
            {
                return true;
            }
            else
                return false;
        }
        public bool CharIn(Point pt, bool ignoreTransparency = false)
        {
            return CharIn(pt.X, pt.Y, ignoreTransparency);
        }

        public bool PointInRoof(int ptx, int pty)
        {
            return PointInRoof(ptx, pty, false);
        }
        public bool PointInRoof(int ptx, int pty, bool ignoreTransparency)
        {
            if (Rectangle.Contains(ptx, pty))
            {
                if (ignoreTransparency == false && (this[ptx - X, pty - Y] == 127 || this[ptx - X, pty - Y] == 0))
                    return false;

                return true;
            }

            return false;
        }
    }
}
