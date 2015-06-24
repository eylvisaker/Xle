using System;

using AgateLib.Serialization.Xle;

using ERY.Xle.Data;
using ERY.Xle.Maps;
using ERY.Xle.Services.Game;
using ERY.Xle.Services.Rendering;

namespace ERY.Xle.Services.MapLoad.Implementation
{
    public class MapLoader : IMapLoader
    {
        private XleData data;
        private IMapExtenderFactory extenderFactory;
        private IEventExtenderFactory eventFactory;
        private IMapRendererFactory rendererFactory;

        public MapLoader(
            XleData data,
            IMapExtenderFactory extenderFactory,
            IEventExtenderFactory eventFactory,
            IMapRendererFactory rendererFactory)
        {
            this.data = data;
            this.extenderFactory = extenderFactory;
            this.eventFactory = eventFactory;
            this.rendererFactory = rendererFactory;
        }

        private string GetFilename(int mapId)
        {
            return "Maps/" + data.MapList[mapId].Filename;
        }

        public MapExtender LoadMap(int mapId)
        {
            string file = GetFilename(mapId);

            return LoadMap(file, mapId);
        }

        public MapExtender LoadMap(string filename, int id)
        {
            XleMap data = LoadMapData(filename, id);

            var extender = extenderFactory.CreateMapExtender(data);
            extender.TheMap = data;
            
            var renderer = extender.CreateMapRenderer(rendererFactory);
            renderer.Extender = extender;
            renderer.TheMap = data;
            extender.MapRenderer = renderer;

            data.MapID = id;

            extender.CreateEventExtenders(eventFactory);

            return extender;
        }

        public XleMap LoadMapData(int mapId)
        {
            string filename = GetFilename(mapId);

            return LoadMapData(filename, mapId);
        }

        private XleMap LoadMapData(string filename, int mapId)
        {
            if (System.IO.Path.GetExtension(filename).ToLower() != ".xmf")
                throw new ArgumentException("File extension not recognized.");

            XleSerializer ser = new XleSerializer(typeof(XleMap));
            ser.Binder = new XleTypeBinder(ser.Binder);

            using (var file = AgateLib.IO.Assets.OpenRead(filename))
            {
                return (XleMap)ser.Deserialize(file);
            }
        }
    }
}
