using System.Collections.Generic;

namespace Xle.Maps
{
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
