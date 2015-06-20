using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.IO;
using AgateLib.Serialization.Xle;

using ERY.Xle.Data;
using ERY.Xle.Maps;
using ERY.Xle.Rendering;

namespace ERY.Xle.Services.Implementation
{
    public class MapLoader : IMapLoader
    {
        private XleData data;
        private IMapExtenderFactory extenderFactory;

        public MapLoader(XleData data, IMapExtenderFactory extenderFactory)
        {
            this.data = data;
            this.extenderFactory = extenderFactory;
        }

        public XleMap LoadMap(int mapId)
        {
            string file = "Maps/" + data.MapList[mapId].Filename;

            return LoadMap(file, mapId);
        }

        public XleMap LoadMap(string filename, int id)
        {
            if (System.IO.Path.GetExtension(filename).ToLower() != ".xmf")
                throw new ArgumentException("File extension not recognized.");

            XleSerializer ser = new XleSerializer(typeof(XleMap));
            ser.Binder = new XleTypeBinder(ser.Binder);

            XleMap result;

            using (var file = FileProvider.Assets.OpenRead(filename))
            {
                result = (XleMap)ser.Deserialize(file);
            }

            result.MapID = id;
            result.mBaseExtender = extenderFactory.CreateMapExtender(result);
            result.mBaseExtender.TheMap = result;
            result.CreateEventExtenders();

            return result;
        }
    }
}
