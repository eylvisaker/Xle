namespace Xle.Bootstrap
{
    //public class FactoryInstaller : IWindsorInstaller
    //{
    //    public void Install(IWindsorContainer container, IConfigurationStore store)
    //    {
    //        container.Kernel.AddFacility<TypedFactoryFacility>();

    //        container.Register(Types.FromAssemblyContaining<IXleFactory>()
    //            .BasedOn<IXleFactory>()
    //            .Configure(Configurer));

    //        container.Register(Types.FromAssembly(WindsorInitializer.MasterAssembly)
    //            .BasedOn<IXleFactory>()
    //            .Configure(x => x.AsFactory()));

    //    }

    //    private void Configurer(ComponentRegistration c)
    //    {
    //        if (c.Implementation == typeof(ICommandFactory))
    //            c.AsFactory(new CommandComponentSelector());
    //        else
    //        {
    //            c.AsFactory();
    //        }

    //    }
    //}
}
