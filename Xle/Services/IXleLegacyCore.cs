using ERY.Xle.Data;
using ERY.Xle.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle
{
    public interface IXleLegacyCore : IXleService
    {
        XleData Data { get; }

        void Redraw();

        void SetTilesAndCommands();

        XleMap LoadMap(int mapId);
    }
}
