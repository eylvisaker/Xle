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
            Map = null;
        }

        public Player Player { get; set; }
        public XleMap Map { get; set; }
        public MapExtender MapExtender
        {
            get
            {
                if (Map == null) 
                    return null; 
                
                return Map.Extender;
            }
        }

        public GameSpeed GameSpeed { get; set; }

    }
}
