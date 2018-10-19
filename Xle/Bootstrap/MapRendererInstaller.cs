namespace Xle.Bootstrap
{
    //public class MapRendererInstaller : IWindsorInstaller
    //{
    //    public void Install(IWindsorContainer container, IConfigurationStore store)
    //    {
    //        PerformRegistration(container, store, WindsorInitializer.MasterAssembly);
    //        PerformRegistration(container, store, this.GetType().Assembly);
    //    }

    //    private void PerformRegistration(IWindsorContainer container, IConfigurationStore store, Assembly assembly)
    //    {
    //        RegisterRendererType<DungeonRenderer>(container, store, assembly);
    //        RegisterRendererType<MuseumRenderer>(container, store, assembly);
    //        RegisterRendererType<OutsideRenderer>(container, store, assembly);
    //        RegisterRendererType<TownRenderer>(container, store, assembly);
    //        RegisterRendererType<TempleRenderer>(container, store, assembly);
    //    }

    //    private void RegisterRendererType<T>(IWindsorContainer container, IConfigurationStore store, Assembly assembly)
    //    {
    //        container.Register(Classes.FromAssembly(assembly)
    //            .BasedOn<T>()
    //            .Configure(c => c.Named(NameOf(c.Implementation)))
    //            .WithServiceBase()
    //            .WithServiceSelf()
    //            .LifestyleSingleton());
    //    }

    //    private string NameOf(Type type)
    //    {
    //        return type.Name;
    //    }
    //}
}