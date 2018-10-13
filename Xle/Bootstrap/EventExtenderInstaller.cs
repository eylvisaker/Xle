namespace ERY.Xle.Bootstrap
{
    //public class EventExtenderInstaller : IWindsorInstaller
    //{
    //    public void Install(IWindsorContainer container, IConfigurationStore store)
    //    {
    //        container.Register(Component.For<EventExtender>().UsingFactoryMethod<EventExtender>(factory).LifestyleTransient());

    //        container.Register(Classes.FromAssembly(WindsorInitializer.MasterAssembly)
    //            .BasedOn<EventExtender>()
    //            .WithServiceSelf()
    //            .Configure(c => c.Named(EventName(c.Implementation)))
    //            .LifestyleTransient());

    //        container.Register(Classes.FromAssemblyContaining<XleColor>()
    //            .BasedOn<EventExtender>()
    //            .WithServiceSelf()
    //            .Configure(c => c.Named(EventName(c.Implementation)))
    //            .LifestyleTransient());
    //    }

    //    private string EventName(Type type)
    //    {
    //        if (type == typeof(StoreRaftExtender))
    //            return "StoreRaft";

    //        return type.Name;
    //    }

    //    private EventExtender factory(IKernel kernel, CreationContext context)
    //    {
    //        var map = context.AdditionalArguments["map"] as MapExtender;
    //        var evt = context.AdditionalArguments["evt"] as XleEvent;
    //        var defaultExtender = context.AdditionalArguments["defaultExtender"] as Type;

    //        var result = CreateNamedEvent(kernel, evt.ExtenderName);
    //        result.TheEvent = evt;

    //        return result;
    //    }

    //    private EventExtender CreateNamedEvent(IKernel kernel, string name)
    //    {
    //        return kernel.Resolve<EventExtender>(name);
    //    }
    //}
}