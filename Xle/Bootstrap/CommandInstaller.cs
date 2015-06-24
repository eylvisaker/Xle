using System.Reflection;

using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

using ERY.Xle.Services;
using ERY.Xle.Services.Implementation.Commands;
using ERY.Xle.Services.Implementation.Commands.MapSpecific;
using Castle.MicroKernel.Context;
using Castle.MicroKernel;

namespace ERY.Xle.Bootstrap
{
    public class CommandInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromAssemblyContaining<Command>()
                .BasedOn<Command>()
                .LifestyleTransient()
                .Configure(NameComponent)
                .WithServiceSelf()
                );

            //container.Register(Component.For<ICommandFactory>()
            //    .AsFactory());

            //container.Register(Types.FromAssemblyContaining<ICommandFactory>()
            //    .BasedOn<ICommandFactory>()
            //    .Configure(x => x.AsFactory()));

        }

        private void NameComponent(ComponentRegistration obj)
        {
            var name = Nameof(obj.Implementation);

            if (string.IsNullOrEmpty(name))
                return;

            obj.Named(name);
        }

        private string Nameof(System.Type type)
        {
            var serviceName = type.GetCustomAttribute<ServiceNameAttribute>();

            if (serviceName == null)
                return "";

            return serviceName.Name ?? "";
        }
    }
}