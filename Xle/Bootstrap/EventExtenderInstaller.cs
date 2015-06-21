using Castle.MicroKernel;
using Castle.MicroKernel.Context;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using ERY.Xle.Maps.Extenders;
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

namespace ERY.Xle.Bootstrap
{
    public class EventExtenderInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<EventExtender>().UsingFactoryMethod<EventExtender>(factory).LifestyleTransient());

            container.Register(Classes.FromAssembly(WindsorInitializer.MasterAssembly)
                .BasedOn<EventExtender>()
                .WithServiceSelf()
                .Configure(c => c.Named(EventName(c.Implementation)))
                .LifestyleTransient());

            container.Register(Classes.FromAssemblyContaining<XleColor>()
                .BasedOn<EventExtender>()
                .WithServiceSelf()
                .Configure(c => c.Named(EventName(c.Implementation)))
                .LifestyleTransient());
        }

        private string EventName(Type type)
        {
            if (type == typeof(StoreRaftExtender))
                return "StoreRaft";

            return type.Name;
        }

        private EventExtender factory(IKernel kernel, CreationContext context)
        {
            var map = context.AdditionalArguments["map"] as MapExtender;
            var evt = context.AdditionalArguments["evt"] as XleEvent;
            var defaultExtender = context.AdditionalArguments["defaultExtender"] as Type;

            return CreateNamedEvent(kernel, evt.ExtenderName);
        }

        private EventExtender CreateNamedEvent(IKernel kernel, string name)
        {
            return kernel.Resolve<EventExtender>(name);
        }
    }
}