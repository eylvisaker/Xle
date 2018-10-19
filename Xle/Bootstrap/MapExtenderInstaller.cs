namespace Xle.Bootstrap
{
    //public class MapExtenderInstaller : IWindsorInstaller
    //{
    //    public void Install(IWindsorContainer container, IConfigurationStore store)
    //    {
    //        container.Register(Component.For<MapExtender>().UsingFactoryMethod<MapExtender>(factory).LifestyleTransient());

    //        container.Register(Classes.FromAssembly(WindsorInitializer.MasterAssembly)
    //            .BasedOn<MapExtender>()
    //            .Configure(c => c.Named(NameOf(c.Implementation)))
    //            .WithServiceSelf()
    //            .LifestyleTransient());
    //    }

    //    private string NameOf(Type type)
    //    {
    //        return type.Name;
    //    }

    //    private MapExtender factory(IKernel kernel, CreationContext context)
    //    {
    //        var xlemap = context.AdditionalArguments["map"] as XleMap;
    //        if (xlemap == null)
    //            return null;

    //        var result = CreateNamedMap(kernel, xlemap.ExtenderName);

    //        result.TheMap = xlemap;

    //        return result;
    //    }

    //    private MapExtender CreateNamedMap(IKernel kernel, string name)
    //    {
    //        return kernel.Resolve<MapExtender>(name);
    //    }
    //}
}
