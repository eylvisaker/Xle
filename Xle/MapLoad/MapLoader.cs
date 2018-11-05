using System;
using AgateLib;
using Xle.Serialization;

using Xle.Data;
using Xle.Maps;
using Xle.Game;
using Xle.Rendering;
using Microsoft.Xna.Framework;

namespace Xle.MapLoad
{
    public interface IMapLoader
    {
        XleMap LoadMapData(int mapId);
        IMapExtender LoadMap(int mapId);
        IMapExtender LoadMap(string filename, int mapId);
    }

    [Singleton]
    public class MapLoader : IMapLoader
    {
        private XleData data;
        private readonly IContentProvider content;
        private IMapExtenderFactory extenderFactory;
        private IEventExtenderFactory eventFactory;
        private IMapRendererFactory rendererFactory;

        public MapLoader(
            XleData data,
            IContentProvider content,
            IMapExtenderFactory extenderFactory,
            IEventExtenderFactory eventFactory,
            IMapRendererFactory rendererFactory)
        {
            this.data = data;
            this.content = content;
            this.extenderFactory = extenderFactory;
            this.eventFactory = eventFactory;
            this.rendererFactory = rendererFactory;
        }

        private string GetFilename(int mapId)
        {
            return "Maps/" + data.MapList[mapId].Filename;
        }

        public IMapExtender LoadMap(int mapId)
        {
            if (data.MapList.ContainsKey(mapId) == false)
                return null;

            string file = GetFilename(mapId);

            return LoadMap(file, mapId);
        }

        public IMapExtender LoadMap(string filename, int id)
        {
            XleMap data = LoadMapData(filename, id);

            var extender = (MapExtender)extenderFactory.CreateMapExtender(data);
            extender.TheMap = data;
            
            var renderer = extender.CreateMapRenderer(rendererFactory);
            renderer.Extender = extender;
            renderer.TheMap = data;
            extender.TheMapRenderer = renderer;

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

            using (var file = content.Open(filename))
            {
                return (XleMap)ser.Deserialize(file);
            }
        }
    }
}
