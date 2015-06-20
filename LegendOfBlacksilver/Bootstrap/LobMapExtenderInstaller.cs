using Castle.MicroKernel;
using Castle.MicroKernel.Context;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using ERY.Xle.LoB.MapExtenders.Archives;
using ERY.Xle.LoB.MapExtenders.Dungeon;
using ERY.Xle.LoB.MapExtenders.Outside;
using ERY.Xle.LoB.MapExtenders.Temples;
using ERY.Xle.LoB.MapExtenders.Towns;
using ERY.Xle.Maps.Extenders;
using ERY.Xle.Maps.XleMapTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.Bootstrap
{
    public class LobMapExtenderInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<MapExtender>().UsingFactoryMethod<MapExtender>(factory).LifestyleTransient());

            container.Register(Classes.FromAssemblyContaining<LobFactory>()
                .BasedOn<MapExtender>()
                .WithServiceSelf()
                .LifestyleTransient());
        }

        private MapExtender factory(IKernel kernel, CreationContext context)
        {
            var xlemap = context.AdditionalArguments["map"];
            var town = xlemap as Town;
            var outside = xlemap as Outside;
            var dungeon = xlemap as Dungeon;
            var museum = xlemap as Museum;
            var castle = xlemap as CastleMap;
            var temple = xlemap as Temple;

            if (town != null)
                return CreateMapExtender(kernel, town);
            if (outside != null)
                return CreateMapExtender(kernel, outside);
            if (dungeon != null)
                return CreateMapExtender(kernel, dungeon);
            if (museum != null)
                return CreateMapExtender(kernel, museum);
            if (temple != null)
                return CreateMapExtender(kernel, temple);

            throw new NotImplementedException();
        }

        private MapExtender CreateMapExtender(IKernel kernel, Temple temple)
        {
            return kernel.Resolve<LobTempleExtender>();
        }

        public TownExtender CreateMapExtender(IKernel kernel, Town town)
        {
            if (town.MapID <= 20)
                return new ThalenTownExtender();
            else
                return new MaelbaneTownExtender();
        }
        public OutsideExtender CreateMapExtender(IKernel kernel, Outside outside)
        {
            if (outside.MapID < 5)
                return new Thalen();
            else
                return new Maelbane();
        }
        public DungeonExtender CreateMapExtender(IKernel kernel, Dungeon theMap)
        {
            switch (theMap.MapID)
            {
                case 61: return new IslandCaverns();
                case 62: return new TaragasMines();
                case 63: return new MarthbaneTunnels();
                case 64: return new PitsOfBlackmire();
                case 65: return new DeathspireChasm();

                default:
                    return kernel.Resolve<DungeonExtender>();
            }
        }
        public MuseumExtender CreateMapExtender(IKernel kernel, Museum museum)
        {
            if (museum.MapID == 53)
                return new OwlArchive();
            else if (museum.MapID == 54)
                return new HawkArchive();

            return kernel.Resolve<MuseumExtender>();
        }
        public CastleExtender CreateMapExtender(IKernel kernel, CastleMap castle)
        {
            return kernel.Resolve<CastleExtender>(castle.ExtenderName);
        }

    }
}
