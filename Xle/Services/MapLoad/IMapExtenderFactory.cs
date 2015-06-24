using ERY.Xle.Maps;
using ERY.Xle.Maps.Extenders;

namespace ERY.Xle.Services.MapLoad
{
    public interface IMapExtenderFactory : IXleFactory
    {
        MapExtender CreateMapExtender(XleMap map);
    }
}
