namespace Xle.Maps
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
}