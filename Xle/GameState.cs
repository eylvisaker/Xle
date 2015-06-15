using ERY.Xle.Maps;
using ERY.Xle.Maps.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle
{
    public class GameState : IXleService
    {
        public GameState()
        {
            Initialize();
        }

        public void Initialize()
        {
            GameSpeed = new GameSpeed();
            Player = null;
            Map = null;
            Commands = null;
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

        public ERY.Xle.Commands.CommandList Commands;

    }
}
