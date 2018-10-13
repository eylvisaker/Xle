using Castle.MicroKernel;
using Castle.MicroKernel.Context;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using ERY.Xle.LoB.MapExtenders.Archives.Exhibits;
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

namespace ERY.Xle.LoB.Bootstrap
{
    //public class LobExhibitInstaller : IWindsorInstaller
    //{
    //    public void Install(IWindsorContainer container, IConfigurationStore store)
    //    {
    //        container.Register(Classes.FromAssemblyContaining<LobExhibit>()
    //            .BasedOn<LobExhibit>()
    //            .LifestyleTransient()
    //            .WithServiceSelf());
    //    }
    //}
}
