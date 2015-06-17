using System.Collections.Generic;

namespace ERY.Xle.Maps
{
	public class MapInfo
	{
		public int ID;
		public string Name;
		public string Filename;
		public int ParentMapID;
		public string Alias;

		public MapInfo(int id, string name, string filename, int parent, string alias)
		{
			ID = id;
			Name = name;
			Filename = filename;
			ParentMapID = parent;
			Alias = alias;
		}
	}
	public class MapList : Dictionary<int, MapInfo>
	{
		public void Add(int id, string name, string filename, int parent, string alias)
		{
			this.Add(id, new MapInfo(id, name, filename, parent, alias));
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
