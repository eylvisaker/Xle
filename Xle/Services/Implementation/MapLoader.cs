using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using AgateLib.DisplayLib;
using AgateLib.Geometry;

using ERY.Xle.Data;
using ERY.Xle.Maps;
using ERY.Xle.Rendering;

namespace ERY.Xle.Services.Implementation
{
    public class MapLoader : IMapLoader
    {
        private XleData data;

        public MapLoader(XleData data)
        {
            this.data = data;
        }

        public XleMap LoadMap(int mapId)
        {
            string file = "Maps/" + data.MapList[mapId].Filename;

            return XleMap.LoadMap(file, mapId);
        }
    }
}
