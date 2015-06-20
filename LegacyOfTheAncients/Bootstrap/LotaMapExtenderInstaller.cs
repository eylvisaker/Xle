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
using ERY.Xle.Maps;
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
                .Configure(c => c.Named(NameOf(c.Implementation)))
                .WithServiceSelf()
                .LifestyleTransient());
        }

        private string NameOf(Type type)
        {
            return type.Name;
        }

        private MapExtender factory(IKernel kernel, CreationContext context)
        {
            var xlemap = context.AdditionalArguments["map"] as XleMap;
            var town = xlemap as Town;
            var outside = xlemap as Outside;
            var dungeon = xlemap as Dungeon;
            var museum = xlemap as Museum;
            var castle = xlemap as CastleMap;

            return CreateNamedMap(kernel, xlemap.ExtenderName);

            if (castle != null)
                return CreateMapExtender(kernel, castle);
            if (outside != null)
                return CreateMapExtender(kernel, outside);
            if (dungeon != null)
                return CreateMapExtender(kernel, dungeon);
            if (museum != null)
                return CreateMapExtender(kernel, museum);
            if (town != null)
                return CreateMapExtender(kernel, town);

            throw new NotImplementedException();
        }

        private MapExtender CreateNamedMap(IKernel kernel, string name)
        {
            return kernel.Resolve<MapExtender>(name);
        }

        public TownExtender CreateMapExtender(IKernel kernel, Town town)
        {
            return kernel.Resolve<LotaTown>();
        }
        public OutsideExtender CreateMapExtender(IKernel kernel, Outside outside)
        {
            if (outside.ExtenderName == "Flight")
                return kernel.Resolve<Flight>();
            else
                return kernel.Resolve<Tarmalon>();
        }
        public DungeonExtender CreateMapExtender(IKernel kernel, Dungeon theMap)
        {
            switch (theMap.MapID)
            {
                case 71: return kernel.Resolve<PiratesLairDungeon>();
                case 72: return kernel.Resolve<ArmakDungeon>();
                case 73: return kernel.Resolve<FourJewelsDungeon>();
            }

            return kernel.Resolve<DungeonExtender>();
        }
        public MuseumExtender CreateMapExtender(IKernel kernel, Museum museum)
        {
            return kernel.Resolve<LotaMuseum>();
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
