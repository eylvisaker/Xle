using AgateLib.Mathematics.Geometry;
using Xle.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xle.XleEventTypes.Stores
{
    public class StoreEquipment : Store
    {
        public StoreEquipment()
        {
            AllowedItemTypes = new List<int>();
        }

        protected override void ReadData(XleSerializationInfo info)
        {
            base.ReadData(info);

            AllowedItemTypes = info.ReadInt32Array("AllowedItemTypes").ToList();
        }
        protected override void WriteData(XleSerializationInfo info)
        {
            base.WriteData(info);

            info.Write("AllowedItemTypes", AllowedItemTypes.ToArray());
        }

        public List<int> AllowedItemTypes { get; set; }
    }
}
