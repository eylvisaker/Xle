using AgateLib.Serialization.Xle;
using ERY.Xle.Maps;
using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes
{
	[Serializable]
	public class Door : XleEvent
	{
		DoorExtender mExtender;

		protected override Extenders.EventExtender CreateExtenderImpl(XleMap map)
		{
			return mExtender = map.CreateEventExtender<DoorExtender>(this);
		}
		protected override void ReadData(XleSerializationInfo info)
		{
			RequiredItem = info.ReadInt32("RequiredItem", 0);
			ReplacementTile = info.ReadInt32("ReplacementTile", 0);
		}
		protected override void WriteData(XleSerializationInfo info)
		{
			info.Write("RequiredItem", RequiredItem);
			info.Write("ReplacementTile", ReplacementTile);
		}

		public int RequiredItem { get; set; }
		public int ReplacementTile { get; set; }


		public void RemoveDoor(GameState state)
		{
			mExtender.RemoveDoor(state);
		}
	}
}
