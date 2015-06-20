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
using ERY.Xle.XleEventTypes;
using ERY.Xle.XleEventTypes.Extenders;
using ERY.Xle.XleEventTypes.Extenders.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.Bootstrap
{
    public class LotaEventExtenderInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<EventExtender>().UsingFactoryMethod<EventExtender>(factory).LifestyleTransient());

            container.Register(Classes.FromAssemblyContaining<LotaFactory>()
                .BasedOn<EventExtender>()
                .WithServiceSelf()
                .LifestyleTransient());

            container.Register(Classes.FromAssemblyContaining<XleColor>()
                .BasedOn<EventExtender>()
                .WithServiceSelf()
                .LifestyleTransient());
        }

        private EventExtender factory(IKernel kernel, CreationContext context)
        {
            var map = context.AdditionalArguments["map"] as MapExtender;
            var evt = context.AdditionalArguments["evt"] as XleEvent;
            var defaultExtender = context.AdditionalArguments["defaultExtender"] as Type;

            var outside = map as OutsideExtender;

            if (map is OutsideExtender)
                return CreateOutsideEvent(kernel, outside, evt, defaultExtender);

            throw new NotImplementedException();
        }

        private EventExtender CreateOutsideEvent(IKernel kernel, OutsideExtender outside, XleEvent evt, Type defaultExtender)
        {
            if (evt is ChangeMapEvent)
                return kernel.Resolve<ChangeMapQuestion>();
            else
                return (EventExtender)kernel.Resolve(defaultExtender);
        }
    }
}