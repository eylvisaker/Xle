using AgateLib.Serialization.Xle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Maps
{
	public class TileGroup : IXleSerializable
	{
		public TileGroup()
		{
			Tiles = new List<int>();
		}

		public List<int> Tiles { get; set; }

		void IXleSerializable.WriteData(XleSerializationInfo info)
		{
			info.Write("Tiles", Tiles.ToArray());
		}

		void IXleSerializable.ReadData(XleSerializationInfo info)
		{
			Tiles = info.ReadList<int>("Tiles");
		}
	}
}
