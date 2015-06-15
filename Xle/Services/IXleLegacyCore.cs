using ERY.Xle.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle
{
    public interface IXleLegacyCore : IXleService
    {
        XleData Data { get; }
        GameState GameState { get; set; }

        void InitializeConsole();

        void Redraw();

        void SetTilesAndCommands();

        Maps.XleMap LoadMap(int p);
    }
}
