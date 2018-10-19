using System.Collections.Generic;

namespace Xle.Data
{
    public class EquipmentList : Dictionary<int, EquipmentInfo>
    {
        public void Add(int id, string name, string prices)
        {
            Add(id, new EquipmentInfo(id, name, prices));
        }
        public string GetName(int id)
        {
            return this[id].Name;
        }
    }
}
