using ERY.Xle.Maps;
using ERY.Xle.Maps.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ERY.Xle.Services;
using ERY.Xle.Services.Implementation.Commands;

namespace ERY.Xle
{
    public class GameState : IXleService
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
        public XleMap Map { get { return MapExtender.TheMap; } }
        public MapExtender MapExtender { get; set; }

        public GameSpeed GameSpeed { get; set; }
    }
}
