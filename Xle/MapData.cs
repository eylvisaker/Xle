using System;
using System.Collections.Generic;
using System.Text;
using AgateLib.Serialization.Xle;

namespace ERY.Xle
{
	public class MapData : IXleSerializable
	{
		int[] mData;
		int mWidth, mHeight;

		#region IXleSerializable Members

		void IXleSerializable.WriteData(XleSerializationInfo info)
		{
			info.Write("Width", mWidth);
			info.Write("Height", mHeight);
			info.Write("Data", mData);
		}

		void IXleSerializable.ReadData(XleSerializationInfo info)
		{
			mWidth = info.ReadInt32("Width");
			mHeight = info.ReadInt32("Height");
			mData = info.ReadInt32Array("Data");
		}

		#endregion
		public int Width
		{
			get { return mWidth; }
		}
		public int Height
		{
			get { return mHeight; }
		}

		MapData()
		{ }
		public MapData(int width, int height)
		{
			mWidth = width;
			mHeight = height;

			mData = new int[mWidth * mHeight];
		}

		public int this[int x, int y]
		{
			get { return mData[x + y * mWidth]; }
			set { mData[x + y * mWidth] = value; }
		}

		/// <summary>
		/// Sets the entire MapData to a single value.
		/// </summary>
		/// <param name="value"></param>
		public void SetValue(int value)
		{
			for (int j = 0; j < Height; j++)
			{
				for (int i = 0; i < Width; i++)
				{
					this[i, j] = value;
				}
			}
		}

	}
}
