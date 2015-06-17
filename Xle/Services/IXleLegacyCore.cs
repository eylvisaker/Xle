using ERY.Xle.Data;
using ERY.Xle.Maps;

namespace ERY.Xle.Services
{
    public interface IXleLegacyCore : IXleService
    {
        XleData Data { get; }

        void Redraw();

        void SetTilesAndCommands();

        XleMap LoadMap(int mapId);
    }
}
