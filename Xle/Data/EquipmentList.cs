using System.Collections.Generic;

namespace ERY.Xle.Data
{
	public class EquipmentInfo
	{
		public int ID { get; private set; }
		public string Name { get; private set; }
		public int[] Prices { get; private set; }
		public bool Ranged { get; set; }

		public EquipmentInfo(int id, string name, string prices)
		{
			ID = id;
			Name = name;

			Prices = new int[5];

			if (string.IsNullOrEmpty(prices) == false)
			{
				string[] vals = prices.Split(',');

				for (int i = 0; i < vals.Length; i++)
				{
					Prices[i] = int.Parse(vals[i]);
				}
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
