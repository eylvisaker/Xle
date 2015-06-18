using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

using ERY.Xle.Services.Implementation.Commands;

namespace ERY.Xle.Bootstrap
{
    public class CommandInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromAssemblyContaining<Command>()
                .BasedOn<Command>()
                .LifestyleTransient()
                .WithServiceSelf());
        }
    }
}