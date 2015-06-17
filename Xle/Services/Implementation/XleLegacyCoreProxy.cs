using ERY.Xle.Data;

namespace ERY.Xle.Services.Implementation
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
