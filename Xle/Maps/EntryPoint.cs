using AgateLib.Geometry;
using AgateLib.Serialization.Xle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Maps
{
    public class EntryPoint : IXleSerializable
    {
        public EntryPoint()
        {
            Facing = Direction.South;
        }
        void IXleSerializable.WriteData(XleSerializationInfo info)
        {
            info.WriteEnum("Facing", Facing, false);
            info.Write("Location", Location);
            info.Write("DungeonLevel", DungeonLevel);
        }

        void IXleSerializable.ReadData(XleSerializationInfo info)
        {
            Facing = info.ReadEnum<Direction>("Facing");
            Location = info.ReadObject<Point>("Location");
            DungeonLevel = info.ReadInt32("DungeonLevel");
        }

        public Direction Facing { get; set; }
        public Point Location { get; set; }
        public int DungeonLevel { get; set; }
    }
}
