using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel;
using Castle.MicroKernel.Context;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

using ERY.Xle.Maps.XleMapTypes;
using ERY.Xle.Services.Rendering.Maps;
using ERY.Xle.XleEventTypes;
using ERY.Xle.XleEventTypes.Extenders;
using ERY.Xle.XleEventTypes.Extenders.Common;
using ERY.Xle.XleEventTypes.Stores.Extenders;
using System.Reflection;

namespace ERY.Xle.Bootstrap
{
    public class MapRendererInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            PerformRegistration(container, store, WindsorInitializer.MasterAssembly);
            PerformRegistration(container, store, this.GetType().Assembly);
        }

        private void PerformRegistration(IWindsorContainer container, IConfigurationStore store, Assembly assembly)
        {
            RegisterRendererType<DungeonRenderer>(container, store, assembly);
            RegisterRendererType<MuseumRenderer>(container, store, assembly);
            RegisterRendererType<OutsideRenderer>(container, store, assembly);
            RegisterRendererType<TownRenderer>(container, store, assembly);
            RegisterRendererType<TempleRenderer>(container, store, assembly);
        }

        private void RegisterRendererType<T>(IWindsorContainer container, IConfigurationStore store, Assembly assembly)
        {
            container.Register(Classes.FromAssembly(assembly)
                .BasedOn<T>()
                .Configure(c => c.Named(NameOf(c.Implementation)))
                .WithServiceBase()
                .WithServiceSelf()
                .LifestyleSingleton());
        }

        private string NameOf(Type type)
        {
            return type.Name;
        }
    }
}