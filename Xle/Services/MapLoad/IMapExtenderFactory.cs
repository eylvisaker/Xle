using Xle.Maps;

namespace Xle.Services.MapLoad
{
    public interface IMapExtenderFactory : IXleFactory
    {
        MapExtender CreateMapExtender(XleMap map);
    }
}
