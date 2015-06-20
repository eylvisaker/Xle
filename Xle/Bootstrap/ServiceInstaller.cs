using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Services;

namespace ERY.Xle.Bootstrap
{
    public class ServiceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromAssemblyContaining<IXleService>()
                .BasedOn<IXleService>()
                .WithServiceSelf()
                .WithServiceAllInterfaces()
                .LifestyleSingleton());

            container.Register(Classes.FromAssembly(WindsorInitializer.MasterAssembly)
                .BasedOn<IXleService>()
                .WithServiceSelf()
                .WithServiceAllInterfaces()
                .LifestyleSingleton());

            container.Register(Component.For<Random>().ImplementedBy<Random>());
        }
    
    }
}
