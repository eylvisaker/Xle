using System;
using System.Collections.Generic;
using System.Text;

namespace ERY.Xle
{
	public class MapInfo
	{
		public int ID;
		public string Name;
		public string Filename;

		public MapInfo(int id, string name, string filename)
		{
			ID = id;
			Name = name;
			Filename = filename;
		}
	}
	public class MapList : Dictionary<int, MapInfo>
	{
		public void Add(int id, string name, string filename)
		{
			this.Add(id, new MapInfo(id, name, filename));
		}

		public string GetName(int id)
		{
			return this[id].Name;
		}

		public string GetFilename(int id)
		{
			return this[id].Filename;
		}
	}
}
