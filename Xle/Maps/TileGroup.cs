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
			AnimateChance = 100;
		}

		public List<int> Tiles { get; set; }

		void IXleSerializable.WriteData(XleSerializationInfo info)
		{
			info.Write("Tiles", Tiles.ToArray(), NumericEncoding.Csv);
			info.Write("AnimationTime", AnimationTime);
			info.WriteEnum("AnimationType", AnimationType, false);
			info.WriteEnum("GroupType", GroupType, false);
			info.Write("AnimateChance", AnimateChance);
		}

		void IXleSerializable.ReadData(XleSerializationInfo info)
		{
			Tiles = info.ReadList<int>("Tiles");
			AnimationType = info.ReadEnum<AnimationType>("AnimationType", Maps.AnimationType.Random);
			AnimationTime = info.ReadInt32("AnimationTime", 50);
			GroupType = info.ReadEnum<GroupType>("GroupType", Maps.GroupType.None);
			AnimateChance = info.ReadInt32("AnimateChance", 100);
		}

		public AnimationType AnimationType { get; set; }
		public int AnimationTime { get; set; }
		public int AnimateChance { get; set; }
		public GroupType GroupType { get; set; }

		public string GroupContents
		{
			get
			{
				if (Tiles.Count == 0)
					return "{empty}";

				return string.Join(",", Tiles.ToArray());
			}
		}

		#region --- Unserialized Properties ---

		public double TimeSinceLastAnim { get; set; }

		#endregion
	}
}
