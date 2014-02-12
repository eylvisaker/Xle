using AgateLib.Serialization.Xle;
using ERY.Xle.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle
{
	public class TileSet : IXleSerializable
	{
		Dictionary<int, TileInfo> mTiles = new Dictionary<int, TileInfo>();

		public TileSet()
		{
			TileGroups = new List<TileGroup>();
		}
		public TileInfo this[int index]
		{
			get { return mTiles[index]; }
			set
			{
				if (index > 5000) throw new ArgumentOutOfRangeException();
				if (index < 0) throw new ArgumentOutOfRangeException();

				mTiles[index] = value;
			}
		}

		public List<TileGroup> TileGroups { get; set; }

		public bool ContainsKey(int value)
		{
			return mTiles.ContainsKey(value);
		}

		void IXleSerializable.WriteData(XleSerializationInfo info)
		{
			List<TileInfo> tiles = new List<TileInfo>();
			
			tiles.Capacity = mTiles.Keys.Max()+1;
			for(int i = 0; i < mTiles.Keys.Max()+1; i++)
				tiles.Add(TileInfo.Normal);

			foreach(int key in mTiles.Keys)
			{
				tiles[key] = mTiles[key];
			}

			info.Write("Tiles", tiles.Select(x => (int)x).ToArray(), NumericEncoding.Csv);
			info.Write("TileGroups", TileGroups);
		}
		void IXleSerializable.ReadData(XleSerializationInfo info)
		{
			int[] tiles = info.ReadInt32Array("Tiles");

			for (int i = 0; i < tiles.Length; i++)
			{
				mTiles[i] = (TileInfo)tiles[i];
			}

			if (info.ContainsKey("TileGroups"))
				TileGroups = info.ReadList<TileGroup>("TileGroups");
		}
	}
}
