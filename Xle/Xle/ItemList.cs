using System;
using System.Collections.Generic;
using System.Text;

namespace ERY.Xle
{
	public class ItemInfo
	{
		public int ID;
		public string Name;
		public string Action;

		public ItemInfo(int id, string name, string action)
		{
			ID = id;
			Name = name;
			Action = action;
		}

		public override string ToString()
		{
			return Name;
		}
	}
	public class ItemList : Dictionary<int, ItemInfo>
	{
		public void Add(int id, string name, string action)
		{
			Add(id, new ItemInfo(id, name, action));
		}
		public string GetName(int id)
		{
			return this[id].Name;
		}
	}
}
