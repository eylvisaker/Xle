using AgateLib;
using AgateLib.Foundation;
using Xle.Maps;
using Xle.Services.Rendering.Maps;

namespace Xle.Services.Rendering
{
    public interface IMapRendererFactory
    {
        DungeonRenderer DungeonRenderer(MapExtender map, string name = null);
        OutsideRenderer OutsideRenderer(MapExtender map, string name = null);
        IMuseumRenderer MuseumRenderer(MapExtender map, string name = null);
        TempleRenderer TempleRenderer(MapExtender map, string name = null);
        TownRenderer TownRenderer(MapExtender map, string name = null);
        CastleRenderer CastleRenderer(MapExtender map, string name = null);
    }

    [Singleton]
    public class MapRendererFactory : IMapRendererFactory
    {
        private readonly IAgateServiceLocator serviceLocator;

        public MapRendererFactory(IAgateServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public CastleRenderer CastleRenderer(MapExtender map, string name = null)
               => Resolve<CastleRenderer>(map, name);

        public DungeonRenderer DungeonRenderer(MapExtender map, string name = null)
               => Resolve<DungeonRenderer>(map, name);

        public IMuseumRenderer MuseumRenderer(MapExtender map, string name = null)
               => Resolve<IMuseumRenderer>(map, name);

        public OutsideRenderer OutsideRenderer(MapExtender map, string name = null)
               => Resolve<OutsideRenderer>(map, name);

        public TempleRenderer TempleRenderer(MapExtender map, string name = null)
               => Resolve<TempleRenderer>(map, name);

        public TownRenderer TownRenderer(MapExtender map, string name = null)
               => Resolve<TownRenderer>(map, name);


        private T Resolve<T>(MapExtender map, string name) where T : IXleMapRenderer
        {
            T result;

            if (name == null)
            {
                result = serviceLocator.Resolve<T>();
            }
            else
            {
                result = serviceLocator.ResolveNamed<T>(name);
            }

            result.TheMap = map.TheMap;
            return result;
        }
    }
}
