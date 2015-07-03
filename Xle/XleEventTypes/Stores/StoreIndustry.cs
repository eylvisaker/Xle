using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes.Stores
{
    public class StoreIndustry : Store
    {
        protected override void ReadData(AgateLib.Serialization.Xle.XleSerializationInfo info)
        {
            base.ReadData(info);

            Product = info.ReadString("Product");
        }
        protected override void WriteData(AgateLib.Serialization.Xle.XleSerializationInfo info)
        {
            base.WriteData(info);

            info.Write("Product", Product);
        }

        public string Product { get; set; }
    }
}
