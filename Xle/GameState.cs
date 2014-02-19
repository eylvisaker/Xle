using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle
{
	public class GameState
	{

		public GameState()
		{ }

		public GameState(Xle.Player player, XleMap xleMap)
		{
			this.Player = player;
			this.Map = xleMap;
		}

		public Player Player { get; set; }
		public XleMap Map { get; set; }
	}
}
