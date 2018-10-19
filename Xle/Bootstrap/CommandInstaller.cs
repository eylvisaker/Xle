namespace Xle.Bootstrap
{
    //public class CommandInstaller : IWindsorInstaller
    //{
    //    public void Install(IWindsorContainer container, IConfigurationStore store)
    //    {
    //        container.Register(Classes.FromAssembly(WindsorInitializer.MasterAssembly)
    //            .BasedOn<Command>()
    //            .LifestyleTransient()
    //            .Configure(NameComponent)
    //            .WithServiceSelf()
    //            );

    //        container.Register(Classes.FromAssemblyContaining<Command>()
    //            .BasedOn<Command>()
    //            .LifestyleTransient()
    //            .Configure(NameComponent)
    //            .WithServiceSelf()
    //            );
    //    }

    //    private void NameComponent(ComponentRegistration obj)
    //    {
    //        var name = Nameof(obj.Implementation);

    //        if (string.IsNullOrEmpty(name))
    //            return;

    //        obj.Named(name);
    //    }

    //    private string Nameof(System.Type type)
    //    {
    //        var serviceName = type.GetCustomAttribute<ServiceNameAttribute>();

    //        if (serviceName == null)
    //            return "";

    //        return serviceName.Name ?? "";
    //    }
    //}
}