using System.Collections.Generic;

namespace Xle.Data
{
    public class ItemInfo
    {
        public int ID;
        public string Name;
        public string Action;
        public string LongName;
        public bool IsKey;

        public ItemInfo(int id, string name, string longName, string action)
        {
            ID = id;
            Name = name;
            Action = action;

            if (string.IsNullOrEmpty(longName))
                LongName = name;
            else
                LongName = longName;
        }

        public override string ToString()
        {
            return Name;
        }
    }
    public class ItemList : Dictionary<int, ItemInfo>
    {
        public void Add(int id, string name, string longName, string action)
        {
            Add(id, new ItemInfo(id, name, longName, action));
        }
        public string GetName(int id)
        {
            return this[id].Name;
        }

        public bool IsKey(int item)
        {
            return this[item].IsKey;
        }
    }
}
