using System;

namespace Xle.Bootstrap
{
    public interface IServiceLocator
    {
        T Resolve<T>();
        T Resolve<T>(object anonymousObjectArguments);

        T ResolveNamed<T>(string name);
        T ResolveNamed<T>(string name, object anonymousObjectArguments);

        IServiceLocatorScope BeginScope();
    }

    public interface IServiceLocatorScope : IDisposable
    {
        T Resolve<T>();
        T Resolve<T>(object anonymousObjectArguments);
    }
}
