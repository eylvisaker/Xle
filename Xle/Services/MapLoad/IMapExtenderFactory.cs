using ERY.Xle.Maps;

namespace ERY.Xle.Services.MapLoad
{
    public interface IMapExtenderFactory : IXleFactory
    {
        MapExtender CreateMapExtender(XleMap map);
    }
}
