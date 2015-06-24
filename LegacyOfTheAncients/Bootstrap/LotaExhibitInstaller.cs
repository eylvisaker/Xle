using Castle.MicroKernel;
using Castle.MicroKernel.Context;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using ERY.Xle.LotA.MapExtenders.Castle;
using ERY.Xle.LotA.MapExtenders.Dungeons;
using ERY.Xle.LotA.MapExtenders.Fortress;
using ERY.Xle.LotA.MapExtenders.Museum;
using ERY.Xle.LotA.MapExtenders.Museum.MuseumDisplays;
using ERY.Xle.LotA.MapExtenders.Outside;
using ERY.Xle.LotA.MapExtenders.Towns;
using ERY.Xle.Maps.XleMapTypes;
using ERY.Xle.XleEventTypes;
using ERY.Xle.XleEventTypes.Extenders;
using ERY.Xle.XleEventTypes.Extenders.Common;
using ERY.Xle.XleEventTypes.Stores.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.Bootstrap
{
    public class LotaExhibitInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromAssemblyContaining<LotaExhibit>()
                .BasedOn<LotaExhibit>()
                .LifestyleTransient()
                .WithServiceSelf());
        }
    }
}
