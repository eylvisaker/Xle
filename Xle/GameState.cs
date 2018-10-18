using AgateLib;
using ERY.Xle.Maps;

namespace ERY.Xle
{
    [Singleton]
    public class GameState
    {
        public GameState()
        {
            Initialize();
        }

        public void Initialize(Player thePlayer = null)
        {
            GameSpeed = new GameSpeed();
            Player = thePlayer;
            MapExtender = null;
        }

        public Player Player { get; set; }
        public XleMap Map
        {
            get
            {
                if (MapExtender == null)
                    return null;
                return MapExtender.TheMap;
            }
        }

        public IMapExtender MapExtender { get; set; }

        public GameSpeed GameSpeed { get; set; }
    }
}
