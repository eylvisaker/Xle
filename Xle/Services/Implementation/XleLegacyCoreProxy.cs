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

        public XleLegacyCoreProxy(XleCore legacyCore)
        {
            this.legacyCore = legacyCore;
        }

        public XleData Data { get { return XleCore.Data; } }
        public GameState GameState
        {
            get { return XleCore.GameState; }
            set { XleCore.GameState = value; }
        }


        public void InitializeConsole()
        {
            legacyCore.InitializeConsole();
        }

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
