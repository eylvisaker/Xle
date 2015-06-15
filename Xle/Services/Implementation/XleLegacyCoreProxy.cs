using ERY.Xle.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle
{
    public class XleLegacyCoreProxy : IXleLegacyCore
    {
        XleCore legacyCore;

        public XleLegacyCoreProxy(XleCore legacyCore, GameState gameState)
        {
            this.legacyCore = legacyCore;
            XleCore.GameState = gameState;
        }

        public XleData Data { get { return XleCore.Data; } }

        public void Redraw()
        {
            XleCore.Redraw();
        }

        public void SetTilesAndCommands()
        {
            XleCore.SetTilesAndCommands();
        }

        public Maps.XleMap LoadMap(int mapID)
        {
            return XleCore.LoadMap(mapID);
        }
    }
}
