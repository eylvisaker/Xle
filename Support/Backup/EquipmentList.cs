using System;
using System.Collections.Generic;
using System.Text;

namespace ERY.Xle
{
    public class EquipmentInfo
    {
        public int ID;
        public string Name;
        public int BasePrice;

        public EquipmentInfo(int id, string name, int basePrice)
        {
            ID = id;
            Name = name;
            BasePrice = basePrice;
        }
    }
    public class EquipmentList : Dictionary<int, EquipmentInfo>
    {
        public void Add(int id, string name, int basePrice)
        {
            Add(id, new EquipmentInfo(id, name, basePrice));
        }
        public string GetName(int id)
        {
            return this[id].Name;
        }
    }
}
