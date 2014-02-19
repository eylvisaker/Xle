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
			info.Write("Tiles", Tiles.ToArray(), NumericEncoding.Csv);
			info.Write("AnimationTime", AnimationTime);
			info.WriteEnum("AnimationType", AnimationType, false);
			info.WriteEnum("GroupType", GroupType, false);
		}

		void IXleSerializable.ReadData(XleSerializationInfo info)
		{
			Tiles = info.ReadList<int>("Tiles");
			AnimationType = info.ReadEnum<AnimationType>("AnimationType", Maps.AnimationType.Random);
			AnimationTime = info.ReadInt32("AnimationTime", 50);
			GroupType = info.ReadEnum<GroupType>("GroupType", Maps.GroupType.None);
		}

		public AnimationType AnimationType { get; set; }
		public int AnimationTime { get; set; }

		public GroupType GroupType { get; set; }


		#region --- Unserialized Properties ---

		public double TimeSinceLastAnim { get; set; }

		#endregion
	}
}
