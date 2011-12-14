using System;
using System.Collections.Generic;
using System.Text;

namespace ERY.Xle
{
	public class EquipmentInfo
	{
		public int ID;
		public string Name;
		public int[] Prices;

		public EquipmentInfo(int id, string name, string prices)
		{
			ID = id;
			Name = name;

			Prices = new int[5];

			string[] vals = prices.Split(',');

			for (int i = 0; i < vals.Length; i++)
			{
				Prices[i] = int.Parse(vals[i]);
			}
		}
	}
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
