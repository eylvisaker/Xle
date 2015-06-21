using Castle.MicroKernel;
using Castle.MicroKernel.Context;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using ERY.Xle.Maps;
using ERY.Xle.Maps.Extenders;
using ERY.Xle.Maps.XleMapTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.Bootstrap
{
    public class MapExtenderInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<MapExtender>().UsingFactoryMethod<MapExtender>(factory).LifestyleTransient());

            container.Register(Classes.FromAssembly(WindsorInitializer.MasterAssembly)
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

            return CreateNamedMap(kernel, xlemap.ExtenderName);
        }

        private MapExtender CreateNamedMap(IKernel kernel, string name)
        {
            return kernel.Resolve<MapExtender>(name);
        }
    }
}
