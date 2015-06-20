using Castle.MicroKernel;
using Castle.MicroKernel.Context;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using ERY.Xle.LotA.MapExtenders.Castle;
using ERY.Xle.LotA.MapExtenders.Dungeons;
using ERY.Xle.LotA.MapExtenders.Fortress;
using ERY.Xle.LotA.MapExtenders.Museum;
using ERY.Xle.LotA.MapExtenders.Outside;
using ERY.Xle.LotA.MapExtenders.Towns;
using ERY.Xle.Maps.Extenders;
using ERY.Xle.Maps.XleMapTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.Bootstrap
{
    public class LotaMapExtenderInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<MapExtender>().UsingFactoryMethod<MapExtender>(factory).LifestyleTransient());

            container.Register(Classes.FromAssemblyContaining<LotaFactory>()
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

            if (town != null)
                return CreateMapExtender(kernel, town);
            if (outside != null)
                return CreateMapExtender(kernel, outside);
            if (dungeon != null)
                return CreateMapExtender(kernel, dungeon);
            if (museum != null)
                return CreateMapExtender(kernel, museum);

            throw new NotImplementedException();
        }


        public TownExtender CreateMapExtender(IKernel kernel, Town town)
        {
            if (town.ExtenderName == "EagleHollow")
                return kernel.Resolve<EagleHollow>();

            return kernel.Resolve<LotaTown>();
        }
        public OutsideExtender CreateMapExtender(IKernel kernel, Outside outside)
        {
            if (outside.ExtenderName == "Flight")
                return kernel.Resolve<Flight>();
            else
                return kernel.Resolve<TarmalonExtender>();
        }
        public DungeonExtender CreateMapExtender(IKernel kernel, Dungeon theMap)
        {
            switch (theMap.MapID)
            {
                case 71: return kernel.Resolve<PirateExtender>();
                case 72: return kernel.Resolve<ArmakExtender>();
                case 73: return kernel.Resolve<FourJewelsExtender>();
            }

            return kernel.Resolve<DungeonExtender>();
        }
        public MuseumExtender CreateMapExtender(IKernel kernel, Museum museum)
        {
            return kernel.Resolve<LotaMuseumExtender>();
        }
        public CastleExtender CreateMapExtender(IKernel kernel, CastleMap castle)
        {
            if (castle.ExtenderName.ToLowerInvariant() == "castle1")
                return kernel.Resolve<CastleGround>();
            if (castle.ExtenderName.ToLowerInvariant() == "castle2")
                return kernel.Resolve<CastleUpper>();
            if (castle.ExtenderName.ToLowerInvariant() == "fortress1")
                return kernel.Resolve<FortressEntry>();
            if (castle.ExtenderName.ToLowerInvariant() == "fortress2")
                return kernel.Resolve<FortressFinal>();

            return kernel.Resolve<CastleExtender>();
        }

    }
}
