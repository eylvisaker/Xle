using ERY.Xle.Maps;
using ERY.Xle.Maps.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle
{
	public class GameState
	{
		public GameState()
		{
			GameSpeed = new GameSpeed();
		}

		public Player Player { get; set; }
		public XleMap Map { get; set; }
		public MapExtender MapExtender { get { return Map.Extender; } }

		public GameSpeed GameSpeed { get; set; }

		public  ERY.Xle.Commands.CommandList Commands;
	}
}
