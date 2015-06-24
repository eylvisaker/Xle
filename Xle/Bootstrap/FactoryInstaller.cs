using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.Facilities.TypedFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Services;

namespace ERY.Xle.Bootstrap
{
    public class FactoryInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Kernel.AddFacility<TypedFactoryFacility>();

            container.Register(Types.FromAssemblyContaining<IXleFactory>()
                .BasedOn<IXleFactory>()
                .Configure(Configurer));

            container.Register(Types.FromAssembly(WindsorInitializer.MasterAssembly)
                .BasedOn<IXleFactory>()
                .Configure(x => x.AsFactory()));

        }

        private void Configurer(ComponentRegistration c)
        {
            if (c.Implementation == typeof(ICommandFactory))
                c.AsFactory(new CommandComponentSelector());
            else
            {
                c.AsFactory();
            }
            
        }
    }
}
